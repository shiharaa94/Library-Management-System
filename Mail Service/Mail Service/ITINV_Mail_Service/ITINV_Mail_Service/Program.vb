Imports System
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail

Module Program
    Dim dt As New DataTable
    Dim dt1 As New DataTable
    Dim cn As New SqlConnection("Data Source=SLCMPKSQL;Initial Catalog=CMPK_LMS;Persist Security Info=True;User ID=sa;password=welcome@123")
    Dim cmd As New SqlCommand("Select BookName,EMP_EPFNo,EMP_Name,IssueDate,ReturnDate FROM AllissueBooks Where Returndate<='" + Format(Now, "yyyy-MM-dd") + "' and Collect_Status='0'", cn)
    Dim cmd1 As New SqlCommand


    Sub Main()
        Console.WriteLine("Programme Started")
        Console.WriteLine("Finding Delayed Books......")
        cn.Open()
        dt.Load(cmd.ExecuteReader)

        If dt.Rows.Count = 0 Then
            Console.WriteLine("No Any Delayed Books......")

        Else

            Console.WriteLine("Delayed Books Email Sending..........")
            Try
                '// HTML Table data//
                Dim mailBody As String = "<table style=""border-collapse:collaps ; text-align:center;"">"
                mailBody += "<tr style=""color:#555555;"">"

                mailBody += "<td style=""border-color:#5c87b2;  width:200px; border-style:solid; border-width:thin; padding: 5px;""><b>Book Name</b></td>"
                mailBody += "<td style=""border-color:#5c87b2;  width:200px; border-style:solid; border-width:thin; padding: 5px;""><b>EPF No</b></td>"
                mailBody += "<td style=""border-color:#5c87b2;  width:200px; border-style:solid; border-width:thin; padding: 5px;""><b>Employee Name</b></td>"
                mailBody += "<td style=""border-color:#5c87b2;  width:200px; border-style:solid; border-width:thin; padding: 5px;""><b>Issue Date</b></td>"
                mailBody += "<td style=""border-color:#5c87b2;  width:200px; border-style:solid; border-width:thin; padding: 5px;""><b>Need to Be Return</b></td>"
                mailBody += "</tr>"
                For Each row As DataRow In dt.Rows '//dg_Handover is a Data Grid View

                    mailBody += "<tr style=""color:#555555;"">"

                    For Each DC As DataColumn In dt.Columns

                        mailBody += "<td style=""border-color:#5c87b2; width:200px; border-style:solid; border-width:thin; padding: 5px;"">" + row(DC) + "</td>"
                    Next
                    mailBody += "</tr>"
                Next
                mailBody += "</table>"
                '// End Table data//


                '//Email Body  End

                Dim mail As New MailMessage
                Dim smtpserver As New SmtpClient()
                mail.From = New MailAddress("LMS@sl.crystal-martin.com")

                mail.To.Add("shihara.anjana@sl.crystal-martin.com,shanika.madushani@sl.crystal-martin.com,kamodi.priyalal@sl.crystal-martin.com,Chameera.Kumara@sl.crystal-martin.com")
                mail.Subject = "Delayed Book List - Library Management System"
                mail.IsBodyHtml = True
                mail.Body = "Hi User,<br>The Following Books are Delayed to Return. Please inform them to Return Books.. <br><br>" & mailBody & "<br><br>" & "This is an Automatically Generated Email by the Library Management System"
                smtpserver.Port = 25
                smtpserver.Host = "192.168.58.243"
                smtpserver.Credentials = New System.Net.NetworkCredential("gps@sl.crystal-martin.com", "g@p@S$$")
                smtpserver.Send(mail)
                Console.WriteLine("Delayed Books Mail Sent")

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End If
        cn.Close()

    End Sub



End Module
