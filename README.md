# dotnet-coordinates-Insphere

Assignment from Insphere to create a gRPU service that parses the coordinates in the run1.csv file and sends the data to a client that then displays the coordinates.

To run the application, just download and extract the v1 release, and launch "Viewer.exe".
It should handle the running of the server itself, only needing the port to run it on as input.

It is important that after you choose the CSV file to parse the coordinates from, that you set the "Path ID" to read, as the corresponding one in the ID column of the CSV. The default value is set to the only existing path in the example data file "data/run1.csv".

## Known issues
Something I haven't been able to get to the bottom of yet, is that SOME of the time, the first time you modify the of the port to the "settings.json" ends up with two closing brackets "}}" instead of "}".

You will know this is the case if you get an error dialog that you've been unable to connect to the server.

To resolve this, open the "settings.json" file in a text editor and remove the extra "}".