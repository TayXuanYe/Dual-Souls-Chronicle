# <center><b> Dual Souls Chronicle </b></center>
###### <center>An Interactive Live RPG Game</center>

# I. Project Overview
"Dual Souls Chronicle" is an innovative, interactive live-stream RPG developed using C# and the Godot game engine. The core of this project is its unique "dual live-stream collaboration" gameplay. Two characters in the game are controlled by audiences from two different live streams, who must work together to defeat monsters and bosses. This is not just a game; it's a collective, interactive experience designed to give viewers an unprecedented sense of participation and community.

# II.Primary Control: Chat Voting
## 1. Primary Control: Chat Voting
All key decisions in the game will be made through chat-based voting in each live stream. This system ensures that the audience from both streams directly influences the fate of their respective characters.

-   <b>Voting Method</b>: 
Viewers cast their votes by sending specific numerical keywords. <b>(e.g., "1","2","111").</b>
<br>
- <b>Valid Votes</b>: 
A chat message containing only a single digit from a specific set of numbers (e.g., "1", "2"). If the message contains multiple instances of the same valid digit (e.g., "111"), it's still counted as a single vote for that number.
<br>
- <b>Invalid Votes</b>:
<b>A message is invalid if it contains any of the following:</b>
Multiple, different valid digits: "12"
Any non-numerical characters: "1 hello", "nice"
<br>
- <b>Chat Display</b>
 All chat messages, except for the voting keywords, will be displayed in real-time on the corresponding characters, adding a fun, visual element to their personalities.

 ## 2. Character Progression and Strategic Choices
 While combat is automated, audience decisions are crucial for character growth and overall team strategy.
 - <b>Buffs</b>:
 When pass the normal level, three random normal buffs will be dropped. Viewers in both streams will vote on which one to choose. If both audiences vote for the same buff, it will be <b>destroyed</b>, and neither character will receive it. This mechanic forces viewers to not only consider their own gain but also to observe and predict the other stream's choice, encouraging strategic communication or even rivalry.
 - <b>Boss Buffs</b>:
 When pass the boss level, one of the boss buff will be dropped. Viewers in both streams will vote on get it or give it to another character. If both chose to get it the buff will <b>randomly given to any of the character and the buff effect reduce buff (damage) </b>. If both chose give to another character, the boss buff will <b>not given to any character.</b>

# III. Character & Class System
The game currently features four basic classes, each with a unique role and play style, providing diverse strategic options for the audience:

 - <b>Warrior</b>: Medium Attack, Medium Defense. A balanced and steady choice, suitable for various situations.

 - <b>Mage</b>: High Attack, Low Defense. Excels at quickly eliminating enemies but requires protection from teammates.

 - <b>Cleric</b>: Low Attack, Low Defense. Their primary role is to heal teammates, ensuring the team's longevity.

 - <b>Shield guard</b>: Low Attack, High Defense. Acts as the team's solid wall, effectively absorbing damage.

# IV. Technical Implementation
 - <b>Game Engine</b>: Godot. Chosen for its lightweight, open-source nature and ease of development, making it perfect for an indie project.

 - <b>Programming Language</b>: C#. Provides robust programming capabilities and excellent support within the Godot engine.

 - <b>Live-Stream Integration</b>: Initially, we plan to integrate with YouTube by using its API or SDK to access chat data. Future plans include expanding support to platforms like Twitch and Bilibili.