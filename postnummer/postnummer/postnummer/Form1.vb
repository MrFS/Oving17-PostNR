
Imports System.ComponentModel
Imports MySql.Data.MySqlClient


Public Class Form1

    Dim command As MySqlCommand

    Public tilkobling = New MySqlConnection("Server=localhost;Database=oving17vb;Uid=root;Pwd=root")


    Private Function sporring(ByVal sql As String) As DataTable
        Dim mydata As New DataTable
        Try
            tilkobling.Open()
            Dim kommando As New MySqlCommand(sql, tilkobling)
            Dim da As New MySqlDataAdapter
            da.SelectCommand = kommando
            da.Fill(mydata)
            tilkobling.Close()
        Catch ex As Exception
            MessageBox.Show("Noe gikk galt. " & ex.Message)
            tilkobling.Close()
        End Try
        Return mydata

    End Function


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        For Each line In System.IO.File.ReadAllLines("postnr.txt")
            ListBox1.Items.Add(line)
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim filnavn = "postnr.txt"
        Dim filnummer = FreeFile()
        FileOpen(filnummer, filnavn, OpenMode.Input)

        While Not EOF(filnummer)
            Dim linje = LineInput(filnummer)

            Dim kolonner = linje.Split(vbTab)

            Dim postnr = kolonner(0)
            Dim postnrOppd = postnr.Split(vbTab)
            Dim poststed = postnrOppd(postnrOppd.Length - 1)
            Dim nr = postnrOppd(0)
            For indeks = 0 To postnrOppd.Length - 2
                nr = nr & " " & postnrOppd(indeks)
            Next
            ListBox1.Items.Add(nr)
            'ListBox1.Items.Add(poststed)


            tilkobling.Open()
            Dim sql As New MySqlCommand("insert into postnummer (postnummer) values (@postnummer)", tilkobling)
            sql.Parameters.AddWithValue("@postnummer", nr)

            sql.ExecuteNonQuery()
            'Dim ansattID = sql.LastInsertedId
            tilkobling.close()

        End While



        FileClose(filnummer)




    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim filnavn = "postnr.txt"
        Dim filnummer = FreeFile()
        FileOpen(filnummer, filnavn, OpenMode.Input)

        While Not EOF(filnummer)
            Dim linje = LineInput(filnummer)

            Dim kolonner = linje.Split(vbTab)

            Dim postnr = kolonner(1)
            Dim postnrOppd = postnr.Split(vbTab)
            Dim poststed = postnrOppd(postnrOppd.Length - 1)
            Dim nr = postnrOppd(0)
            For indeks = 0 To postnrOppd.Length - 1
                nr = nr & " " & postnrOppd(indeks)
            Next

            ListBox1.Items.Add(poststed)
            tilkobling.Open()
            Dim sql As New MySqlCommand("insert into poststed (poststed) values (@poststed)", tilkobling)
            sql.Parameters.AddWithValue("@poststed", poststed)

            sql.ExecuteNonQuery()

            tilkobling.close()
        End While



        FileClose(filnummer)

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Dim dbdataset As New DataTable
        Dim bsource As New BindingSource
        Dim SDA As New MySqlDataAdapter


        Try
            tilkobling.open()
            Dim query As String
            query = "select postnummer, poststed from postnummer, poststed where postnummer.id = poststed.id order by postnummer asc"
            Command = New MySqlCommand(query, tilkobling)
            SDA.SelectCommand = command
            SDA.Fill(dbdataset)
            bsource.DataSource = dbdataset
            DataGridView1.DataSource = bsource
            SDA.Update(dbdataset)
            tilkobling.close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            tilkobling.dispose()

        End Try
    End Sub

    Private Sub ToolStripTextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ToolStripTextBox1.KeyDown

        Dim sok = ToolStripTextBox1.Text

        Select Case e.KeyCode
            Case Keys.Enter

                Dim dbdataset As New DataTable
                Dim bsource As New BindingSource
                Dim SDA As New MySqlDataAdapter


                Try
                    tilkobling.open()
                    Dim query As String
                    query = "SELECT postnummer, poststed from postnummer, poststed where postnummer.id = poststed.id and (postnummer.postnummer like '%" & sok & "%' or poststed.poststed like '%" & sok & "%');"
                    command = New MySqlCommand(query, tilkobling)
                    SDA.SelectCommand = command
                    SDA.Fill(dbdataset)
                    bsource.DataSource = dbdataset
                    DataGridView1.DataSource = bsource
                    SDA.Update(dbdataset)
                    tilkobling.close()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                Finally
                    tilkobling.dispose()

                End Try



        End Select
    End Sub
End Class
