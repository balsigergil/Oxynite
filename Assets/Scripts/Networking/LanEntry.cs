﻿/// ETML
/// Author: Gil Balsiger
/// Date: 16.05.2018
/// Summary: Contains informations about a LAN server

/// <summary>
/// Contains informations about a LAN server
/// </summary>
public struct LanEntry {

    public string ipAddress;
    public string hostName;
    public int port;

    public LanEntry(string ip, string host)
    {
        ipAddress = ip;
        hostName = host;
        port = 7777;
    }
	
}
