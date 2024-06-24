# dotnet-coordinates-Insphere

Assignment from Insphere to create a gRPU service that parses the coordinates in the run1.csv file and sends the data to a client that then displays the coordinates.

TODO:
- Create client for displaying the coordinates (NOTE: test that the resources are being correctly released after client consumption - connections are staying open with gRPCurl)
- Investigate "Message.Parser" for reading the file