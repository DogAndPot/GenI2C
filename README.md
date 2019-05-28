# GenI2C

An Automatic tool to get your DSDT ready for VoodooI2C

## Facts

- This tool is written in **Visual Basic .Net**, so it won't work on macOS
- Since there are too many situations in the DSDT, it's possible that there are some which aren't covered in this program. **If you discovered one, please open an issue**
- I'm not a professional programmer and I'm sure there are lots of stupid algorithms in it, giving a suggestion by opening an issue will be highly appreciated

## Supported Situations

| DSDT Situations                     | APIC Pin | Interrupt Support                                                                      | Polling Support                                                               |
|:-----------------------------------:|:--------:|:--------------------------------------------------------------------------------------:|:-----------------------------------------------------------------------------:|
|                                     |          |                                                                                        |                                                                               |
| Native APIC + Native GPIO           | < 2F     | Yes (APIC)                                                                             | No                                                                            |
| Native APIC + No GPIO               | =        | =                                                                                      | =                                                                             |
| Native Combined APIC + Native GPIO  | =        | =                                                                                      | =                                                                             |
| Native Combined APIC + No GPIO      | =        | =                                                                                      | =                                                                             |
|                                     |          |                                                                                        |                                                                               |
| Native APIC + Native GPIO           | > 2F     | Yes (GPIO)                                                                             | Yes                                                                           |
| Native APIC + No GPIO               | =        | =                                                                                      | =                                                                             |
| Native Combined APIC + Naitive GPIO | =        | =                                                                                      | =                                                                             |
| Native Combined APIC + No GPIO      | =        | =                                                                                      | =                                                                             |
|                                     |          |                                                                                        |                                                                               |
| No APIC + No GPIO                   | No       | Yes (Will ask to provide Pin,will fall back to APIC interrupt if possible APIC & GPIO) | Yes (Will genetate a APIC with Pin 3F)                                        |
| No APIC + Native GPIO               | =        | Yes (Won't fall back to APIC if possible, GPIO only)                                   | =                                                                             |
|                                     |          |                                                                                        |                                                                               |
| Native Combined APIC + Native GPIO  | Unknown  | =                                                                                      | Yes (Will fall back to APIC interrupt if possible, depends on system filling) |
| Native APIC + No GPIO               | =        | Yes (Will ask to provide Pin, APIC & GPIO)                                             | =                                                                             |
| Native Combined APIC + NO GPIO      | =        | =                                                                                      | =                                                                             |
| Native APIC + Native GPIO           | =        | =                                                                                      | Yes                                                                           |

## To do

- [ ] Solve errors when returns in `_CRS` contain `I2CM`, `I2CX`
- [ ] Check and patch `SSCN` & `FMCN`
- [ ] Generate SSDT
- [ ] Add a friendly interface (Perhaps)
- [ ] **Translate the project into Swift 5**

## Credits

- Bat.bat [(@williambj1)](https://github.com/williambj1) for the idea and the whole project
- [Alexandred](https://github.com/alexandred) for VoodooI2C [(Full Credits)](https://voodooi2c.github.io/#Credits%20and%20Acknowledgments/Credits%20and%20Acknowledgments)
- Startpenghubingzhou [@penghubingzhou](https://github.com/penghubingzhou) for providing theoretical support and his fancy DSDT
- Steve Zheng [(@stevezhengshiqi)](https://github.com/stevezhengshiqi) for testing and bug reporting
- http://patorjk.com for the amazing ASCII Art font `Impossible`