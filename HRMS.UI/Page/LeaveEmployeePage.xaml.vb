Imports System.Collections.ObjectModel
Imports System.Net.Http
Imports DocuSign.eSign.Model
Imports Newtonsoft.Json

Public Class LeaveEmployeePage : Implements IPage
    Private httpclient As HttpClient


    Public Sub New()
        InitializeComponent()
        httpclient = New HttpClient()



    End Sub
    Public Function Add() As Task Implements IPage.Add
        Throw New NotImplementedException()
    End Function

    Public Function Delete() As Task Implements IPage.Delete
        Throw New NotImplementedException()
    End Function

    Public Function Update() As Task Implements IPage.Update
        Throw New NotImplementedException()
    End Function

    Private Async Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Using client As New HttpClient()
            Dim response As HttpResponseMessage
            Dim selectedstart = employeeleaveStartDate.SelectedDate
            Dim selectedend = employeeleaveEndDate.SelectedDate

            Dim formattedselectedstart As String = selectedstart?.ToString("yyyy-MM-dd")
            Dim formattedselectedend As String = selectedend?.ToString("yyyy-MM-dd")

            response = Await client.GetAsync($"https://localhost:5030/Leave/GetLeavesWithinDateRange?startdate={formattedselectedstart}&enddate={formattedselectedend}")

            If response.IsSuccessStatusCode Then
                Dim json As String = Await response.Content.ReadAsStringAsync()
                Dim wrapper = JsonConvert.DeserializeObject(Of DataWrapper)(json)

                If wrapper.Data IsNot Nothing AndAlso wrapper.Data.Any() Then
                    employeeleaveGridControl.ItemsSource = wrapper.Data
                Else
                    MessageBox.Show("Belirtilen tarih aralığında veri bulunamadı.", "Bilgi", MessageBoxButton.OK, MessageBoxImage.Information)
                    employeeleaveGridControl.ItemsSource = New ObservableCollection(Of Leave)()
                End If
            Else
                ' Hata durumunu ele alın
                MessageBox.Show("Veri alınırken bir hata oluştu.", "Hata", MessageBoxButton.OK, MessageBoxImage.Error)
                employeeleaveGridControl.ItemsSource = New ObservableCollection(Of Leave)()
            End If
        End Using
    End Sub

    Public Class DataWrapper
        Public Property Data As ObservableCollection(Of Leave)
    End Class
End Class
