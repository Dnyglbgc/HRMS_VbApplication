Imports HRMS.Model
Imports HRMS.Model.Models
Imports Infrastructure

Public Interface ILeaveRepository : Inherits IBaseRepository(Of Leaf, Long)
    Function ManageLeave(leaveId As Long?, startDate As DateTime, endDate As DateTime, status As String, employeeId As Long, leaveTypeId As Long, operation As String) As Task(Of String)
    Function GetLeavesDateRange(startDate As DateTime, endDate As DateTime, Optional includeList As List(Of String) = Nothing) As Task(Of List(Of Leaf))
End Interface
