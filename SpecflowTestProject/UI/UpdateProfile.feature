@update @ui @generalinformation
feature: updateprofile
  in order to set new information about myself in newbookmodels
  as a client of newbookmodels
  i want update my profile in newbookmodels

@positive 
scenario outline: update generalinfo with valid data
	given client is created
	and update profile page is opened
	when i update profile with data on updateprofile page
	| name   | last_name   | industry   | company_location   |
	| <name> | <last_name> | <industry> | <company_location> |
	then successfully changed name to <new_name>, industry to <industry>, company location to <company_location> on update profile page
examples:
	| name    | last_name | industry | company_location                          | new_name       |
	| lili    | bom       | fashion  | gatlinburg, tn 37738, usa                 | lili bom       |