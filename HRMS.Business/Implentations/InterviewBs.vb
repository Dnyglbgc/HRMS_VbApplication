﻿Imports AutoMapper
Imports HRMS.Model
Imports HRMS.Model.Models
Imports HRMS.Repository
Imports Infrastructure
Imports Infrastructure.Infrastructure.Utilities.ApiResponses

Public Class InterviewBs : Implements IInterviewBs
    Private ReadOnly _repo As IInterviewRepository
    Private ReadOnly _mapper As IMapper

    Public Sub New(repo As IInterviewRepository, mapper As IMapper)
        _repo = repo
        _mapper = mapper
    End Sub
    Public Async Function GetById(id As Long) As Task(Of ApiResponse(Of InterviewGetDto)) Implements IInterviewBs.GetById
        Dim interview = Await _repo.GetByIdAsync(id)
        Dim mappedInterview = _mapper.Map(Of InterviewGetDto)(interview)

        Return ApiResponse(Of InterviewGetDto).Success(200, mappedInterview)
    End Function

    Public Async Function Add(dto As InterviewPostDto) As Task(Of ApiResponse(Of InterviewGetDto)) Implements IInterviewBs.Add
        Dim newItem = _mapper.Map(Of Interview)(dto)
        Dim added = Await _repo.AddAsync(newItem)

        Dim newDto = _mapper.Map(Of InterviewGetDto)(added)

        Return ApiResponse(Of InterviewGetDto).Success(201, newDto)
    End Function

    Public Async Function GetAll() As Task(Of ApiResponse(Of IEnumerable(Of InterviewGetDto))) Implements IInterviewBs.GetAll
        Dim repoResponse = Await _repo.GetAllAsync()
        Dim dtoList = _mapper.Map(Of IEnumerable(Of InterviewGetDto))(repoResponse)

        Return ApiResponse(Of IEnumerable(Of InterviewGetDto)).Success(200, dtoList)
    End Function

    Public Async Function Delete(id As Long) As Task(Of ApiResponse(Of NoData)) Implements IInterviewBs.Delete
        Dim result = Await _repo.DeleteAsync(id)
        If result Then
            Return ApiResponse(Of NoData).Success(200)
        Else
            Return ApiResponse(Of NoData).Fail(404, "Item not found")
        End If
    End Function

    Public Async Function Update(id As Long, dto As InterviewPutDto) As Task(Of ApiResponse(Of InterviewGetDto)) Implements IInterviewBs.Update
        Dim existingItem = Await _repo.GetByIdAsync(id)
        If existingItem Is Nothing Then
            Return ApiResponse(Of InterviewGetDto).Fail(404, "Item not found")
        End If

        _mapper.Map(dto, existingItem)
        Await _repo.UpdateAsync(existingItem)

        Dim updatedDto = _mapper.Map(Of InterviewGetDto)(existingItem)

        Return ApiResponse(Of InterviewGetDto).Success(200, updatedDto)
    End Function
End Class
