########How to Run the solution############
Make sure you have the visual studio installed
Make sure you have .net 6 sdk and runtime installed
Open the solution LatencyService.sln
Press ctrl+f5
Use the swagger page http://localhost:5257/swagger/index.html to test out

########Assumptions made#############
You can only request upto 30 days data
You can only request data in 2021
Duplicate request ids are not possible in the same day for the same service, but can happen in different services or different days.
Average latency is calculated to the nearest ms.


#######Skipped due to time limitations####
Error/Exception handling
Logging
Integration tests
Not all the functionalities are unit tested. Only critical functionalities are unit tested.