﻿Imports HRMS.Model.Models
Imports Infrastructure
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore
Imports System.Data
Public Class LeaveRepository : Inherits BaseRepository(Of Leaf, Long, HRMSContext) : Implements ILeaveRepository

    Private ReadOnly _context As HRMSContext ' Replace with your actual DbContext

    Public Sub New(context As HRMSContext)
        MyBase.New(context)
        _context = context
    End Sub


    Public Async Function ManageLeave(leaveId As Long?, startDate As DateTime, endDate As DateTime, status As String, employeeId As Long, leaveTypeId As Long, operation As String) As Task(Of String) Implements ILeaveRepository.ManageLeave
        Return Await Task.Run(Function()
                                  Using conn As New SqlConnection(_context.Database.GetDbConnection().ConnectionString)
                                      Using cmd As New SqlCommand("sp_ManageLeaves", conn)
                                          cmd.CommandType = CommandType.StoredProcedure
                                          cmd.Parameters.Add(New SqlParameter("@LeaveID", If(leaveId.HasValue, leaveId.Value, DBNull.Value)))
                                          cmd.Parameters.Add(New SqlParameter("@StartDate", startDate))
                                          cmd.Parameters.Add(New SqlParameter("@EndDate", endDate))
                                          cmd.Parameters.Add(New SqlParameter("@Status", status))
                                          cmd.Parameters.Add(New SqlParameter("@EmployeeID", employeeId))
                                          cmd.Parameters.Add(New SqlParameter("@LeaveTypeID", leaveTypeId))
                                          cmd.Parameters.Add(New SqlParameter("@Operation", operation))

                                          conn.Open()
                                          Dim result As Object = cmd.ExecuteScalar()
                                          conn.Close()

                                          Return If(result IsNot Nothing, result.ToString(), "Error: No result returned from stored procedure.")
                                      End Using
                                  End Using
                              End Function)
    End Function

    Public Async Function GetLeavesDateRange(startDate As DateTime, endDate As DateTime, Optional includeList As List(Of String) = Nothing) As Task(Of List(Of Leaf)) Implements ILeaveRepository.GetLeavesDateRange

        If _context Is Nothing Then

            Return New List(Of Leaf)()
        End If

        Dim queryable As IQueryable(Of Leaf) = _context.Leaves

        If includeList IsNot Nothing Then
            For Each include In includeList
                queryable = queryable.Include(include)
            Next
        End If

        Return Await queryable.
       Where(Function(l) l.Startdate >= startDate AndAlso l.Enddate <= endDate).
       ToListAsync()

    End Function
End Class