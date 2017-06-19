for example


                    UDPCom.Instance.SetUDPInfo(udpIp.ToString(), port);
                    UDPCom.Instance.Send("{\"u\":" + UserInfo.Instance.UID + ",\"r\":" + Room._roomId + "}");