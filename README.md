# SeedPacket
SeedPacket adds a .seed() method onto IEnumerable for quickly seeding data. Similar to a LINQ extension, it populates lists with realistic data with a simple .Seed() to most IEnumerables.<br />
<br />
It is easy to use, with a customizable, and powerful rules engine that keys off the datatype or interface, and name of an item's properties so that the data is appropriate to the type. That is to say, "out-of-the-box" - email properties will be filled with valid emails, phone numbers filled with phone numbers, and names are names etc. If you need to modify the default generated data the rules are simple to create and modify, and come with a many examples, including a data generator that pulls from an xml or json file or string. The randomly generated data can be set to always be repeatable or to be random on each request.
<br />
<img src="[https://www.seedpacket.net/Content/Images/SeedPacketImage.png](https://localhost:7001/images/SeedPacketImage.png)" align="right" />
<br />
<i>// Creates 10 rows (default)</i><br />
var users = new List<User>().Seed();<br />
<br />
<i>// Create 20 rows</i><br />
var users = new List<User>().Seed(20);<br />
<br />
<i>// Create rows starting with 100 to 200</i><br />
var users = new List<User>().Seed(100, 200);<br />
<br />
For more information with extensive examples <br />
please see the documentation at:<br />
<br />
http://www.seedpacket.net<br />
