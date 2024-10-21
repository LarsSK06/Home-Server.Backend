namespace HomeServer.Utilities;

public struct Generator{

    public static int GetEpoch(){
        return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
    }

}