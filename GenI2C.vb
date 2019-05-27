Imports System.IO
Imports System.Text
Module GenI2C

    Public TPAD, Device, DSDTFile, Paranthesesopen, Paranthesesclose, DSDTLine, Spacing, APICNAME, SLAVName, GPIONAME As String
    Dim Code(), ManualGPIO(8), ManualAPIC(6) As String
    Public Matched, CRSPatched, ExUSTP, ExSSCN, ExFMCN, ExAPIC, ExSLAV, ExGPIO, CatchSpacing, APICNameLineFound, SLAVNameLineFound, GPIONameLineFound, InterruptEnabled, PollingEnabled, Hetero As Boolean
    Public line, i, total, APICPinLine, GPIOPinLine, APICPIN, GPIOPIN, GPIOPIN2, APICNameLine, SLAVNameLine, GPIONAMELine, CheckConbLine As Integer

    Sub Main()
        Try
            Console.WriteLine("")
            Console.WriteLine("         _              _            _              _        _                _      ")
            Console.WriteLine("        /\ \           /\ \         /\ \     _     /\ \    /\ \             /\ \     ")
            Console.WriteLine("       /  \ \         /  \ \       /  \ \   /\_\   \ \ \  /  \ \           /  \ \    ")
            Console.WriteLine("      / /\ \_\       / /\ \ \     / /\ \ \_/ / /   /\ \_\/ /\ \ \         / /\ \ \   ")
            Console.WriteLine("     / / /\/_/      / / /\ \_\   / / /\ \___/ /   / /\/_/\/_/\ \ \       / / /\ \ \  ")
            Console.WriteLine("    / / / ______   / /_/_ \/_/  / / /  \/____/   / / /       / / /      / / /  \ \_\ ")
            Console.WriteLine("   / / / /\_____\ / /____/\    / / /    / / /   / / /       / / /      / / /    \/_/ ")
            Console.WriteLine("  / / /  \/____ // /\____\/   / / /    / / /   / / /       / / /  _   / / /          ")
            Console.WriteLine(" / / /_____/ / // / /______  / / /    / / /___/ / /__     / / /_/\_\ / / /________   ")
            Console.WriteLine("/ / /______\/ // / /_______\/ / /    / / //\__\/_/___\   / /_____/ // / /_________\  ")
            Console.WriteLine("\/___________/ \/__________/\/_/     \/_/ \/_________/   \________/ \/____________/  ")
            'http://patorjk.com Impossible
            Console.WriteLine("")
            While True
                Console.Write("File Path (Drag and Drop the dsl file into the Form) : ")
                DSDTFile = Console.ReadLine()
                Try
                    If Dir(DSDTFile) <> "" Then
                        If InStr(Dir(DSDTFile), ".dsl") > 0 Then
                            Exit While
                        ElseIf InStr(Dir(DSDTFile), ".aml") > 0 Then
                            Console.WriteLine("AML files aren't supported! Please input again!")
                            Console.WriteLine()
                        Else
                            Console.WriteLine("Unknown File! Please input again!")
                            Console.WriteLine()
                        End If
                    Else
                        Console.WriteLine("File doesn't exist, please input again!")
                        Console.WriteLine()
                    End If
                Catch ex As Exception
                    Console.WriteLine("Illegal Characters exists, please input again!")
                    Console.WriteLine()
                End Try
            End While
            Console.WriteLine("")
            Console.WriteLine("Search for a Device")
            While True
                Console.WriteLine()
                Console.Write("Device: ")
                TPAD = Console.ReadLine()
                If Len(TPAD) = 4 Then
                    Exit While
                Else
                    Console.WriteLine()
                    Console.WriteLine("Please Input your device name correctly (e.g. " & Chr(34) & "TPD0" & Chr(34) & ")!")
                End If
            End While

            Device = "Device (" & TPAD & ")"
            Matched = False
            line = 0
            i = 0
            total = 0
            Countline()
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub Countline()
        Try
            FileOpen(1, DSDTFile, OpenMode.Input, OpenAccess.Read)
            While Not EOF(1)

                DSDTLine = LineInput(1)
                line = line + 1
                If InStr(DSDTLine, "If (USTP)") > 0 Then
                    Console.WriteLine("Found for USTP in DSDT at line " & line)
                    ExUSTP = True
                    'Else
                End If
                If InStr(DSDTLine, "SSCN") > 0 Then
                    Console.WriteLine("Found for SSCN in DSDT at line " & line)
                    ExSSCN = True
                    'Else
                End If
                If InStr(DSDTLine, "FMCN") > 0 Then
                    Console.WriteLine("Found for FMCN in DSDT at line " & line)
                    ExFMCN = True
                    'Else
                End If
                If InStr(DSDTLine, Device) > 0 Then
                    Dim spaceopen, spaceclose, startline As Integer
                    startline = line
                    Paranthesesopen = LineInput(1)
                    line = line + 1
                    spaceopen = InStr(Paranthesesopen, "{")
                    Do
                        Paranthesesclose = LineInput(1)
                        spaceclose = InStr(Paranthesesclose, "}")
                        line = line + 1
                    Loop Until spaceclose = spaceopen
                    If total = 0 Then
                        total = total + (line - startline)
                    Else total = total + (line - startline) + 1
                    End If
                    If spaceclose = spaceopen Then
                        Matched = True
                    End If
                End If
            End While
            FileClose()
            If Matched = False Then
                Console.WriteLine()
                Console.WriteLine("This is not a Device that exists in the DSDT")
                Console.WriteLine("Exiting")
                Console.ReadLine()
                End
            Else
                Console.WriteLine()
                Analysis()
            End If
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub Analysis()
        Try
            ReDim Code(total)
            FileOpen(1, DSDTFile, OpenMode.Input, OpenAccess.Read)
            While Not EOF(1)
                DSDTLine = LineInput(1)
                If InStr(DSDTLine, Device) > 0 Then
                    Dim spaceopen, spaceclose As Integer
                    Code(i) = DSDTLine
                    i = i + 1
                    Paranthesesopen = LineInput(1)
                    Code(i) = Paranthesesopen
                    i = i + 1
                    spaceopen = InStr(Paranthesesopen, "{")
                    Do
                        Paranthesesclose = LineInput(1)
                        spaceclose = InStr(Paranthesesclose, "}")
                        Code(i) = Paranthesesclose
                        i = i + 1
                    Loop Until spaceclose = spaceopen
                    Matched = True
                End If
            End While
            FileClose()

            For i = 0 To total
                If InStr(Code(i), "GpioInt") > 0 Then
                    If ExGPIO = True Then
                        ExGPIO = False
                        GPIONameLineFound = False
                    End If
                    Console.WriteLine("Native GpioInt Found in " & TPAD & " at line " & i)
                    ExGPIO = True
                    GPIONAMELine = i
                    For GPIONAMELine = GPIONAMELine To 1 Step -1
                        If InStr(Code(GPIONAMELine), "Name (SBF") > 0 And GPIONameLineFound = False Then
                            GPIONAME = Code(GPIONAMELine).Substring((InStr(Code(GPIONAMELine), "SBF") - 1), 4)
                            GPIONameLineFound = True
                        End If
                    Next
                    GPIOPinLine = i + 4
                    GPIOPIN = Convert.ToInt32(Code(GPIOPinLine).Substring(InStr(Code(GPIOPinLine), "0x") - 1), 16)
                    Console.WriteLine("GPIO Pin " & GPIOPIN)
                End If
                If InStr(Code(i), "Interrupt (ResourceConsumer") > 0 Then
                    If ExAPIC = True Then APICNameLineFound = False
                    Console.WriteLine("Native APIC Found in " & TPAD & " at line " & i)
                    ExAPIC = True
                    APICNameLine = i
                    For APICNameLine = APICNameLine To 1 Step -1
                        If InStr(Code(APICNameLine), "Name (SBF") > 0 And APICNameLineFound = False Then
                            APICNAME = Code(APICNameLine).Substring((InStr(Code(APICNameLine), "SBF") - 1), 4)
                            APICNameLineFound = True
                        End If
                    Next
                    APICPinLine = i + 2
                    APICPIN = Convert.ToInt32(Code(APICPinLine).Substring(InStr(Code(APICPinLine), "0x") - 1, 10), 16)
                    Console.WriteLine("APIC Pin " & APICPIN)
                End If
                If InStr(Code(i), "I2cSerialBusV2 (0x") > 0 Then
                    If ExSLAV = True Then SLAVNameLineFound = False
                    Console.WriteLine("Slave Address Found in " & TPAD & " at line " & i)
                    ExSLAV = True
                    SLAVNameLine = i
                    For SLAVNameLine = SLAVNameLine To 1 Step -1
                        If InStr(Code(SLAVNameLine), "Name (SBF") > 0 And SLAVNameLineFound = False Then
                            SLAVName = Code(SLAVNameLine).Substring((InStr(Code(SLAVNameLine), "SBF") - 1), 4)
                            SLAVNameLineFound = True
                            CheckConbLine = SLAVNameLine
                        End If
                    Next
                End If
                If InStr(Code(i), "Name (") > 0 And CatchSpacing = False Then
                    Spacing = Code(i).Substring(0, InStr(Code(i), "Name (") - 1)
                    CatchSpacing = True
                End If
                If SLAVNameLineFound = True And APICNameLineFound = True And SLAVName = APICNAME And Hetero = False Then
                    Hetero = True
                    BreakConbine()
                End If
            Next

            If SLAVNameLineFound = False Then
                Console.WriteLine()
                Console.WriteLine("This is not a I2C Trackpad!")
                Console.WriteLine("Exiting")
                Console.ReadLine()
                End
            End If

            Console.WriteLine()
            Console.WriteLine("Choose the mode you'd like to patch")
            Console.WriteLine()
            Console.WriteLine("1) Interrupt (APIC or GPIO)")
            Console.WriteLine("2) Polling (Will be set back to APIC if supported)")
            Console.WriteLine()
            Console.Write("Selection: ")
            Dim Choice As Integer = Console.ReadLine()
            If Choice = 1 Then
                InterruptEnabled = True
                Console.WriteLine()
            ElseIf Choice = 2 Then
                PollingEnabled = True
                Console.WriteLine()
            Else
                Console.WriteLine()
                Console.WriteLine("Undefined Behaviour, Exiting")
                Console.ReadLine()
                End
            End If

            If ExAPIC = True And ExGPIO = False And APICPIN > 47 Then
                If InterruptEnabled = True Then
                    Console.WriteLine("No native GpioInt found, Generating instead")
                    GPIONAME = "SBFG"
                    APIC2GPIO()
                    PatchCRS2GPIO()
                ElseIf PollingEnabled = True Then
                    If Hetero = True Then APICNAME = "SBFX"
                    PatchCRS2APIC()
                End If
            ElseIf ExAPIC = True And APICPIN <= 47 And APICPIN >= 24 Then '<= 0x2F Group A & E
                Console.WriteLine("APIC Pin value < 2F, Native APIC Supported, using instead")
                If Hetero = True Then APICNAME = "SBFX"
                PatchCRS2APIC()
            ElseIf ExAPIC = True And ExGPIO = True And (APICPIN > 47 Or APICPIN = 0) Then
                If InterruptEnabled = True Then
                    PatchCRS2GPIO()
                ElseIf PollingEnabled = True Then
                    If APICPIN = 0 Then
                        Console.WriteLine("APIC Pin size uncertain, could be either APIC or polling")
                    End If
                    If Hetero = True Then APICNAME = "SBFX"
                    PatchCRS2APIC()
                End If
            ElseIf ExAPIC = False And ExGPIO = True Then
                If InterruptEnabled = True Then
                    PatchCRS2GPIO()
                End If
                If PollingEnabled = True Then
                    APICPIN = 63
                    APICNAME = "SBFX"
                    PatchCRS2APIC()
                End If
            ElseIf ExAPIC = True And ExGPIO = False And APICPIN = 0 Then
                If InterruptEnabled = True Then
                    Console.WriteLine("Failed to extract APIC Pin, filled by system start up. Please input your APIC Pin in Hex")
                    Console.Write("APIC Pin: ")
                    APICPIN = Convert.ToInt32(Console.ReadLine(), 16)
                    While APICPIN < 24 Or APICPIN > 119
                        Console.WriteLine()
                        Console.WriteLine("APIC Pin out of range!")
                        Console.WriteLine("Select your choice:")
                        Console.WriteLine("1) Input again")
                        Console.WriteLine("2) Exit")
                        Console.WriteLine()
                        Console.Write("Your Choice: ")
                        If Console.ReadLine() = 1 Then
                            Console.Write("APIC Pin: ")
                            APICPIN = Convert.ToInt32(Console.ReadLine(), 16)
                        ElseIf Console.ReadLine() = 2 Then
                            Console.WriteLine("Exiting")
                            Console.ReadLine()
                            End
                        Else
                            Console.WriteLine("Unknown Behaviour, Exiting")
                            Console.ReadLine()
                            End
                        End If
                    End While
                    If APICPIN >= 24 And APICPIN <= 47 Then
                        Console.WriteLine("APIC Pin value < 2F, Native APIC Supported, using instead")
                        If Hetero = True Then APICNAME = "SBFX"
                        PatchCRS2APIC()
                    Else
                        GPIONAME = "SBFG"
                        APIC2GPIO()
                        PatchCRS2GPIO()
                    End If
                ElseIf PollingEnabled = True Then
                    Console.WriteLine("APIC Pin size uncertain, could be either APIC or polling")
                    If Hetero = True Then APICNAME = "SBFX"
                    PatchCRS2APIC()
                End If
            ElseIf ExAPIC = False And ExGPIO = False And ExSLAV = True Then ' I don't think this situation exists
                If InterruptEnabled = True Then
                    Console.WriteLine("No native APIC found, failed to extract APIC Pin. Please input your APIC Pin in Hex")
                    Console.Write("APIC Pin: ")
                    APICPIN = Convert.ToInt32(Console.ReadLine(), 16)
                    While APICPIN < 24 Or APICPIN > 119
                        Console.WriteLine()
                        Console.WriteLine("APIC Pin out of range!")
                        Console.WriteLine("Select your choice:")
                        Console.WriteLine("1) Input again")
                        Console.WriteLine("2) Exit")
                        Console.WriteLine()
                        Console.Write("Your Choice: ")
                        If Console.ReadLine() = 1 Then
                            Console.Write("APIC Pin: ")
                            APICPIN = Convert.ToInt32(Console.ReadLine(), 16)
                        Else
                            Console.WriteLine("Undefined Behaviour, Exiting")
                            Console.ReadLine()
                            End
                        End If
                    End While
                    If APICPIN >= 24 And APICPIN <= 47 Then
                        Console.WriteLine("APIC Pin value < 2F, Native APIC Supported, No _CRS Patch required")
                    Else
                        GPIONAME = "SBFG"
                        APIC2GPIO()
                        PatchCRS2GPIO()
                    End If
                ElseIf PollingEnabled = True Then
                    APICNAME = "SBFI"
                    PatchCRS2APIC()
                End If
            Else
                Console.WriteLine("Undefined Situation")
                Console.ReadLine()
                End
            End If
            GenSSDT()
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub PatchCRS2GPIO()
        Try
            Dim CRSLine, CRSReturnline As Integer
            For CRSLine = 0 To total
                If InStr(Code(CRSLine), "Method (_CRS") > 0 Then ' Find _CRS
                    For CRSReturnline = CRSLine To (total - 2) ' Change All Returns in _CRS to GpioInt Name
                        If InStr(Code(CRSReturnline), "Return (ConcatenateResTemplate") > 0 Then
                            Code(CRSReturnline) = Code(CRSReturnline).Substring(0, InStr(Code(CRSReturnline), ", SBF") - 1) & ", " & GPIONAME & "))"
                            CRSPatched = True
                        ElseIf InStr(Code(CRSReturnline), "Return (SBF") > 0 Then
                            ' Capture “Spaces & 'Return'” inject "ConcatenateResTemplate", add original return method name, add GpioInt Name                       
                            Code(CRSReturnline) = Code(CRSReturnline).Substring(0, InStr(Code(CRSReturnline), "(") - 1) & "(ConcatenateResTemplate (" & SLAVName & ", " & GPIONAME & ")) // Usually this return won't function, please check your Windows Patch"
                            CRSPatched = True
                        End If
                    Next
                End If
            Next
            If CRSPatched = False Then Console.WriteLine("Error! No _CRS Patch Applied!")
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub PatchCRS2APIC()
        Try
            Dim CRSLine, CRSReturnline As Integer
            For CRSLine = 0 To total
                If InStr(Code(CRSLine), "Method (_CRS") > 0 Then ' Find _CRS
                    For CRSReturnline = CRSLine To (total - 2) ' Change All Returns in _CRS to APIC Name
                        If InStr(Code(CRSReturnline), "Return (ConcatenateResTemplate") > 0 Then
                            Code(CRSReturnline) = Code(CRSReturnline).Substring(0, InStr(Code(CRSReturnline), ", SBF") - 1) & ", " & APICNAME & "))"
                            CRSPatched = True
                        ElseIf InStr(Code(CRSReturnline), "Return (SBF") > 0 Then
                            ' Capture “Spaces & 'Return'” inject "ConcatenateResTemplate", add original return method name, add APIC Name
                            Code(CRSReturnline) = Code(CRSReturnline).Substring(0, InStr(Code(CRSReturnline), "(") - 1) & "(ConcatenateResTemplate (" & SLAVName & ", " & APICNAME & ")) // Usually this return won't function, please check your Windows Patch"
                            CRSPatched = True
                        End If
                    Next
                End If
            Next
            If CRSPatched = False Then Console.WriteLine("Error! No _CRS Patch Applied!")
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub APIC2GPIO()
        Try
            If APICPIN >= 24 And APICPIN <= 47 Then '< 0x2F Group A & E
                Console.WriteLine("APIC Pin value < 2F, Native APIC Supported, Generation Cancelled")
            ElseIf APICPIN > 47 And APICPIN <= 79 Then '0x30 Group B & F
                GPIOPIN = APICPIN - 24
                GPIOPIN2 = APICPIN + 72
            ElseIf APICPIN > 79 And APICPIN <= 119 Then '0x50
                GPIOPIN = APICPIN - 24
            End If
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub GenGPIO()
        Try
            ManualGPIO(0) = Spacing & "Name (SBFG, ResourceTemplate ()"
            ManualGPIO(1) = Spacing & "{"
            ManualGPIO(2) = Spacing & "    GpioInt (Level, ActiveLow, Exclusive, PullUp, 0x0000,"
            ManualGPIO(3) = Spacing & "       " & Chr(34) & "\\ _SB.PCI0.GPI0," & Chr(34) & "0x00, ResourceConsumer, ,"
            ManualGPIO(4) = Spacing & "        )"
            ManualGPIO(5) = Spacing & "        {   // Pin list"
            If GPIOPIN2 = 0 Then
                ManualGPIO(6) = Spacing & "            0x" & Hex(GPIOPIN)
            Else
                ManualGPIO(6) = Spacing & "            0x" & Hex(GPIOPIN) & " // Try this if the first one doesn't work: 0x" & Hex(GPIOPIN2)
            End If
            ManualGPIO(7) = Spacing & "        }"
            ManualGPIO(8) = Spacing & "})"
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub GenAPIC()
        Try
            If (PollingEnabled = True And Hetero = True) Or ExAPIC = False Then
                ManualAPIC(0) = Spacing & "Name (SBFX, ResourceTemplate ()"
            ElseIf InterruptEnabled = True And Hetero = True Then
                ManualAPIC(0) = Spacing & "Name (SBFX, ResourceTemplate ()"
            Else
                ManualAPIC(0) = Spacing & "Name (SBFI, ResourceTemplate ()"
            End If
            ManualAPIC(1) = Spacing & "{"
            ManualAPIC(2) = Spacing & "    Interrupt (ResourceConsumer, Level, ActiveHigh, Exclusive, ,, )"
            ManualAPIC(3) = Spacing & "    {"
            ManualAPIC(4) = Spacing & "        0x000000" & Hex(APICPIN) & ","
            ManualAPIC(5) = Spacing & "    }"
            ManualAPIC(6) = Spacing & "})"
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub GenSSDT()
        Try
            Dim GenIndex As Integer
            'Dim path As String = Path & TPAD & ".dsl"
            'Dim fs As FileStream = File.Create(path)
            Console.WriteLine()
            Console.WriteLine("Copy the Code from here and replace it into your DSDT")
            Console.WriteLine()
            Console.WriteLine("*****************************************************")
            Console.WriteLine()
            Console.WriteLine(Code(0))
            Console.WriteLine(Code(1))
            If InterruptEnabled = True And ExGPIO = False And APICPIN > 47 Then
                GenGPIO()
                For GenIndex = 0 To 8
                    Console.WriteLine(ManualGPIO(GenIndex))
                Next
            End If
            If (PollingEnabled = True And ExAPIC = False) Or Hetero = True Then
                GenAPIC()
                For GenIndex = 0 To 6
                    Console.WriteLine(ManualAPIC(GenIndex))
                Next
            End If
            For i = 2 To total
                'Dim info As Byte() = New UTF8Encoding(True).GetBytes(Code(i) & vbCrLf)
                'fs.Write(info, 0, info.Length)
                Console.WriteLine(Code(i))
            Next
            Console.WriteLine()
            Console.WriteLine("*****************************************************")
            Console.WriteLine()
            Console.WriteLine("Copy the Code above and replace it into your DSDT")
            Console.WriteLine()
            Console.WriteLine("Enjoy!")
            Console.WriteLine("Type in " & Chr(34) & "Exit" & Chr(34) & " to exit")
            While True
                If Console.ReadLine() = "Exit" Then End
            End While
            'fs.Close()
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub

    Sub BreakConbine()
        Try
            For CheckConbLine = (CheckConbLine + 6) To (CheckConbLine + 9)
                Code(CheckConbLine) = ""
            Next
        Catch ex As Exception
            Console.WriteLine()
            Console.WriteLine("Unknown error, please open an issue and provide your files")
            Console.WriteLine("Exiting")
            Console.ReadLine()
            End
        End Try
    End Sub
End Module' to do: SSCN FMCN, I2CM, Generate SSDT(DefinitionBlock)