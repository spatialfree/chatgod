using UnityEngine;
using System;

public class Monolith : MonoBehaviour
{
  public TwitchIRC IRC;
  public Chatter latestChatter;
  private void Awake()
  {
    IRC.newChatMessageEvent.AddListener(NewMessage);
  }

  public Rig rig = new Rig();

  [Header("References")]
  public Camera headsetCam;

  void Start()
  {
    rig.Start(this);
  }

  void Update()
  {
    rig.Update();
  }

  public Peer[] peers = new Peer[64];
  [Serializable]
  public class Peer
  {
    // state
    public float lastTime;

    // info
    public string name;

    // data
    public string msg;
    public Vector3 pos;
  }

  public Peer GetPeer(string name)
  {
    float time = Mathf.Infinity;
    Peer oldestPeer = null;
    for (int i = peers.Length - 1; i >= 0; i--)
    {
      if (peers[i].name == name)
      {
        return peers[i];
      }
      else if (peers[i].lastTime < time)
      {
        oldestPeer = peers[i];
      }
    }

    oldestPeer.name = name;
    return oldestPeer;
  }

  public void NewMessage(Chatter chatter)
  {
    Debug.Log("New chatter object received! " + chatter.tags.displayName);

    Peer peer = GetPeer(chatter.tags.displayName);
    peer.lastTime = Time.time;

    peer.msg = chatter.message;
    // do something
    // say something (beep-boops)
    // change face
    // move etc.



    latestChatter = chatter;
  }
}


// FAT

// if (chatter.tags.displayName == "Lexone")
//   Debug.Log("Chat message was sent by Lexone!");

// if (chatter.HasBadge("subscriber"))
//   Debug.Log("Chat message sender is a subscriber");

// if (chatter.HasBadge("moderator"))
//   Debug.Log("Chat message sender is a channel moderator");

// if (chatter.MessageContainsEmote("25"))
//   Debug.Log("Chat message contained the Kappa emote");

// if (chatter.message == "!join")
//   Debug.Log(chatter.tags.displayName + " said !join");

// Color nameColor = chatter.GetRGBAColor();

// Check chatter's display name for unusual characters
//
// This can be useful to check for because most fonts don't support unusual characters
// If that's the case then you could use their login name instead (chatter.login) or use a fallback font
// Login name is always lowercase and can only contain characters: a-z, A-Z, 0-9, _
//
// if (chatter.CheckDisplayName())
//   Debug.Log("Chatter's displayName contains characters other than a-z, A-Z, 0-9, _");