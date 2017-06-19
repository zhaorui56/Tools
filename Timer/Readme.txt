
for example 

MyTimeManager bind on one Object
MyTimeManager.StartAll();

int timeid = MyTimeManager.Instance.CreateTimer(second, times, pertime, send,
                                                (j, param) =>
                                                {
							// j is timer id    
							// param is send
                                                });

MyTimeManager.Instance.StartTimer(timeid );