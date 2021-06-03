﻿@update @ui @phoneNumber
Feature: ChangePhoneNumber
  In order to set new phone in NewBookModels
  As a client of NewBookModels
  I want update my profile in NewBookModels

@positive
Scenario Outline: It is possible to change phone number with valid data
	Given Client is created
	And Update Profile page is opened
	When I change phone with data on Update Profile page
	| new_phone   |
	| <new_phone> |
	Then Successfully changed phone to <new_phone> on update profile page
Examples:
	| new_phone  |
	| 123.456.7890 |