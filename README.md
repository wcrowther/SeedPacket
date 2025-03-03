# SeedPacket
SeedPacket extends IEnumerable with a .seed() method for quickly populating lists with realistic data, similar to a LINQ extension.

Its powerful, customizable rules engine uses data types, interfaces, and property names to generate appropriate values�emails for email fields, phone numbers for phone fields, and so on. Default rules work out-of-the-box, but you can easily create or modify them, with examples provided, including data pulled from XML or JSON. Generated data can be repeatable or randomized per request.

<img src="/SeedPacket.Examples/wwwroot/images/SeedPacketImage.png" align="right" />

***// Creates 10 rows (default)***<br />
**var users = new List<User>().Seed();**

***// Create 20 rows***<br />
**var users = new List<User>().Seed(20);**

***// Create rows starting with 100 to 200***<br />
**var users = new List<User>().Seed(100, 200);**

*For more information with extensive examples 
please see the documentation at:
http://www.seedpacket.net*
