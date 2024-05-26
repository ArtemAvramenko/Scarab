using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

public static class UpdateDraft
{
    private static string BaseDir { get; set; }

    private static readonly Regex _rtlRegex = new Regex("[\u0591-\u07FF]");

    private static readonly Dictionary<string, string> _forcedCodeToSignMap = new Dictionary<string, string>()
        {
            { "USD", "$" },
            { "JPY", "¥" },
            { "GBP", "£" },
            { "KRW", "₩" },
            { "SGD", "S$" }
        };

    private static Lazy<List<XmlDocument>> _localeDocs = new Lazy<List<XmlDocument>>(() =>
    {
        var res = new List<XmlDocument>();
        var dir = new DirectoryInfo(Path.Combine(BaseDir, "main"));
        foreach (var fileInfo in dir.GetFiles("*.xml"))
        {
            if (fileInfo.FullName.EndsWith("brx.xml"))
            {
                // The Boro language contains many devanagari signs that are unreadable to other cultures
                continue;
            }
            var xml = new XmlDocument { XmlResolver = null };
            xml.LoadXml(File.ReadAllText(fileInfo.FullName));
            res.Add(xml);
        }
        return res;
    });

    private static Lazy<HashSet<string>> _actualCodes = new Lazy<HashSet<string>>(() =>
    {
        var res = new HashSet<string>();
        var xml = new XmlDocument { XmlResolver = null };
        xml.LoadXml(File.ReadAllText(Path.Combine(BaseDir, "supplemental", "supplementalData.xml")));

        foreach (XmlNode node in xml.SelectNodes("//supplementalData/currencyData/region/currency"))
        {
            var code = node.Attributes["iso4217"].Value.ToUpperInvariant();
            var from = node.Attributes["from"];
            var to = node.Attributes["to"];

            if (from != null && to == null)
            {
                res.Add(code);
            }
        }
        return res;
    });

    private static HashSet<string> GetDuplicatedSymbols()
    {
        var actualCodes = _actualCodes.Value;
        var signToCurrency = new Dictionary<string, string>();
        var skippedSigns = new HashSet<string>();

        foreach (var xml in _localeDocs.Value)
        {
            foreach (XmlNode node in xml.SelectNodes("//ldml/numbers/currencies/currency"))
            {
                var code = node.Attributes["type"].Value.ToUpperInvariant();
                if (!actualCodes.Contains(code))
                {
                    continue;
                }
                var symbols = new HashSet<string>();
                foreach (XmlNode symbolNode in node.SelectNodes("symbol"))
                {
                    symbols.Add(symbolNode.InnerText);
                }
                foreach (var symbol in symbols)
                {
                    string oldCode;
                    if (!signToCurrency.TryGetValue(symbol, out oldCode))
                    {
                        signToCurrency.Add(symbol, code);
                    }
                    else if (oldCode != code)
                    {
                        skippedSigns.Add(symbol);
                    }
                }
            }
        }

        return skippedSigns;
    }

    private static Dictionary<string, List<string>> GetCodeToSignMap()
    {
        var actualCodes = _actualCodes.Value;
        var codeToSigns = new Dictionary<string, List<string>>();
        var skippedSigns = GetDuplicatedSymbols();

        foreach (var xml in _localeDocs.Value)
        {
            foreach (XmlNode node in xml.SelectNodes("//ldml/numbers/currencies/currency"))
            {
                var code = node.Attributes["type"].Value.ToUpperInvariant();
                if (!actualCodes.Contains(code))
                {
                    continue;
                }

                foreach (XmlNode symbolNode in node.SelectNodes("symbol"))
                {
                    var sign = symbolNode.InnerText;
                    string forcedSign;
                    _forcedCodeToSignMap.TryGetValue(code, out forcedSign);
                    if (sign == code ||
                        _rtlRegex.IsMatch(sign) ||
                        skippedSigns.Contains(sign) && sign != forcedSign)
                    {
                        continue;
                    }

                    List<string> oldSignList;
                    if (!codeToSigns.TryGetValue(code, out oldSignList))
                    {
                        oldSignList = new List<string>();
                        codeToSigns[code] = oldSignList;
                    }
                    if (!oldSignList.Contains(sign))
                    {
                        oldSignList.Add(sign);
                    }
                }
            }
        }

        foreach (var list in codeToSigns.Values)
        {
            list.Sort(StringComparer.Ordinal);
        }
        return codeToSigns;
    }

    public static void Generate(string cldrDir, string filePath)
    {
        BaseDir = cldrDir;
        var res = new List<string>();
        foreach (var pair in GetCodeToSignMap())
        {
            res.Add(pair.Key + ',' + string.Join("|", pair.Value.ToArray()));
        }
        res.Sort(StringComparer.Ordinal);
        File.WriteAllText(filePath, string.Join("\n", res));
    }

    public static void Main()
    {
        Generate(
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CLDR_common"),
            "../../../draft.csv");
    }
}
