namespace SpaceTrucker.Shared.Models;
public enum GameEventType
{
    None,
    ClientConnect,
    ClientConnectAccepted,
    ShipMove,
    ShipDock,
    ShipUndock,
    PlayerBuy,
    PlayerSell,
    PlayerConnect,
    PlayerDisconnect,
    PlayerUpdate,
    PlayerList,
    ChatMessage

}