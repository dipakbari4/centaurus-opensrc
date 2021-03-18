Option Strict On
Option Explicit On

Public Class FAQ
    Private ReadOnly answerBox() As String =
        {"The Centaurus is an intelligent multiplayer launcher for I.G.I.-2 game based on .NET Framework 4.5." +
        " It was founded on 26th January 2020. The official method to play in earlier day was only" +
        " the GameSpy, which was abandoned in 2014. This service is powered by I.G.I.-2 HOMECOMING.",
 _
        "Nope. The Centaurus is not an official app launched by Codemasters. Nevertheless of not being" +
        " official, there are lots of facilities have been provided which really makes it great and unique.",
 _
        "Of course. The multiplayer is still active and players come to play in it. The most common time" +
        " noticed for the gameplay is between 4:00 pm - 12:00 am (GMT+5:30).",
 _
        "There are two reasons and solutions behind it. If you are seeing it on every startup, then" +
        " it clearly means that your IP is blocked from the server. To solve it, you need to contact an admin" +
        " through the Complaint Form found when you get a Sync Error.",
 _
        "This service runs under the cooperation of I.G.I.-2 HOMECOMING. The gameplay server is originally" +
        " hosted by <{GH[*]$t}> (in-game name) and the software and facilities are provider by LinuX Man (in-game name).",
 _
        "The most probable reason of hanging Centaurus almost everytime is nothing but slower network connections." +
        " To solve it, try reducing the network hogging from other resources or disable features from Edit Features.",
 _
        "You can change your in-game name on every 250 joins. This rule has been applied for fairness and preventing" +
        " the usage of irrelevant names by the players. Once you hit 250 milestone, you will be able to change your name once.",
 _
        "You need to learn the basics of tactics, stealth and mission execution. You must do the objectives" +
        " within the given limited time. Do not lose courage so easily because you are not going to keep losing" +
        " forever if you were been unsuccessful. Practicing at least an hour per day will make you a" +
        " perfect player within 6 months.",
 _
        "You can use our Feedback (Ctrl+Sft+F), 'Contact an Admin' or Complaint Form (from 'sync-error' page)." +
        " Note that sync-error page will only be displayed when the server data is not fetched from the Centaurus." +
        " You get contact an admin through Centaurus directly (located under Community section).",
 _
        "This is a well-received question. You can take the help of ChatUp (Ct+Sft+C), message there," +
        " if someone will stay active in the Cen, they will get notified about your message. As soon as they see" +
        " they will join. Another option is to 'Wait for Somebody' feature when there are no players. You can join" +
        " our community to call the players in your time.",
 _
        "The server does unexpectedly crashes when the explosives such as LAW-80, Grenades, etc." +
        " are used too much. Therefore, they are blocked by the server. But no worries! You can enjoy" +
        " all those guns and grenades every full Saturday night and Sunday when they are unlocked.",
 _
        "When you do not have the required map to play which is currently running on the server, the" +
        " 'igi2.exe' will be keep crashing with a black screen and displaying 'Server found, loading'." +
        " To fix it, use MapDown (Ctrl+Sft+M) to install the required map.",
 _
        "The Centaurus is an automated system. You will be notified if any new announcement is released." +
        " You can read the announcement anytime from Announcements (Ct+Sft+A). Same with app updates.",
 _
        "The time for gameplay is fixed between 5:00 pm - 11:00 pm (GMT+5:30), but still the players" +
        " have many things in real world to do. They cannot manage to come everyday, thus, you need to call them" +
        " or sometimes you do not need it either.",
 _
        "The I.G.I.-2 HOMECOMING is a public gaming community intended fundamentally to make I.G.I.-2 alive. The community" +
        " provides all the required tools, resources & instructions to guide newbies and support" +
        " the players when they get any errors or have problems.",
 _
        "If you really want to contribute us, write us some suggestions (i.e. feedback us from Community option)."}

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Label4.Text = answerBox(ComboBox1.SelectedIndex)
    End Sub
End Class