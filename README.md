# ¤ SCARAB: Symbol Collection and Repository of All Banknotes

This project aims to create a database of currency icons and their corresponding display formats without being culturally specific. The motivation behind this project is to address the problem of cultural differences in currency representation and the resulting ambiguity. By identifying invariant icons that uniquely identify currency, regardless of culture, this dataset can be used to improve the consistency of currency representations and avoid misinterpretation.

## Background

The way currencies are displayed can vary significantly from country to country, even for the same currency. In addition, different currencies may have the same icon, which is even more confusing. The goal of this project is to identify unique and globally recognizable currency symbols that can be understood by all. The study was conducted by collecting data on currency icons and display formats, taking into account factors such as the position of the symbol (before or after the amount) and the inclusion of a space.

## Dataset Construction

The dataset was created using a two-step process:

1. Draft Generation: Common Locale Data Repository (CLDR) data was used as the basis for defining currency representation formats. This helped identify widely accepted conventions for currency representation. 
2. Manual validation: The initial selection was manually validated and refined by cross-comparing the identified display formats with data available online. This step ensured the accuracy and reliability of the dataset.

## Dataset Format

The dataset is represented as a table, where each record consists of a three-letter currency code (according to ISO 4217) as the key and one or more display formats as the value. The display formats represent different ways of placing the currency symbol in relation to the amount. The formats are chosen to ensure unique and unambiguous identification of each currency. Here are examples from the dataset:

| Code | CultureReference | Signs               |
|------|------------------|---------------------|
| AUD  | en-AU            | A$0                 |
| BRL  | pt-BR            | R$ 0                |
| CZK  | cs-CZ            | 0 Kč                |
| EUR  | de-DE            | 0 €                 |
| PLN  | pl-PL            | 0 zł                |
| TRY  | tr-TR            | ₺0\|0 TL            |
| UAH  | uk-UA            | 0 ₴\|0 грн.\|0 hrn. |
| ZAR  | en-ZA            | R 0                 |

## Usage Considerations

Before using this dataset, it is important to make sure that the selected currency display formats are supported by the software or platform being used. It is recommended to check if the selected designations are available in the required software or font set. For example, some default fonts such as Arial may not contain all required currency symbols. A compatibility check should be performed to ensure smooth integration and correct display of the selected display formats.

## Versioning

Major version matches the CLDR version on which the draft dataset was created.

## Disclaimer

Although this dataset has been carefully selected, it is always advisable to thoroughly test and validate the selected display formats in the intended software or platform. Font compatibility and availability may vary and it is important to ensure consistent and accurate currency mapping.

## Acknowledgments

This project is grateful to the Common Locale Data Repository (CLDR) for providing valuable source data on currency display formats. In addition, internet sources and references obtained through the manual verification process played a critical role in refining the dataset.

## License

This dataset is available under CC BY terms. Please refer to the LICENSE file for more details.
