﻿Imports HRMS.Model
Imports Infrastructure.Infrastructure.Utilities.ApiResponses

Public Interface ILeaveBs
    Function GetByEmployeeId(employeeId As Long) As Task(Of ApiResponse(Of List(Of LeafGetDto)))
    Function ManageLeave(leaveId As Long?, startDate As DateTime, endDate As DateTime, status As String, employeeId As Long, leaveTypeId As Long, operation As String) As Task(Of ApiResponse(Of String))
    Function SubtractAnnualLeave(id As Long, dto As LeafPutDto) As Task(Of ApiResponse(Of LeafPutDto))

    Function GetAll() As Task(Of ApiResponse(Of List(Of LeafGetDto)))
End Interface
