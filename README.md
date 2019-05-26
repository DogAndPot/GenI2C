# GenI2C

An Automatic tool to get your DSDT ready for VoodooI2C

## Facts

- This tool is written in **Visual Basic .Net**, so it won't work on macOS
- Since there are too many circumstances in the DSDT, it's possible that there are some which aren't covered in this program. **If you discovered one, please open an issue**
- I'm not a professional programmer and I'm sure there are lots of stupid algorithms in it, giving a suggestion by opening an issue will be highly appreciated

## To do

- [ ] Solve errors when returns in `_CRS` contain `I2CM`, `I2CX`
- [ ] Check and patch `SSCN` & `FMCN`
- [ ] Generate SSDT
- [ ] Add a friendly interface (Perhaps)
- [ ] **Translate the project into Swift 5**

## Credits

- Bat.bat (@williambj1) for the idea and the whole project
- [Alexandred](https://github.com/alexandred) for VoodooI2C [(Full Credits)](https://voodooi2c.github.io/#Credits%20and%20Acknowledgments/Credits%20and%20Acknowledgments)
- Startpenghubingzhou @penghubingzhou for providing theoretical support and his fancy DSDT
- http://patorjk.com for the amazing ASCII Art font `Impossible`