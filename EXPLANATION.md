# Doom-Eternal-s-Loot-Vacuum
In this single file lies the functionality that I tried to replicate for a shooter that I'm currently developing. It only requires one script to imitate the vacuum-like property of Doom Eternal's loot once dropped by the chainsaw.

FixedUpdate: Here, I run two conditionals to know whether the player is able to Pick up the type of the Pickup (Armor, Health, Shells, Bullets, etc.) and also make sure that the loot orb should follow the player. This means checking the player's inventory to make sure it isn't full and calculate the distance between the player and the orb.

PreparePikcup: This is called upon creation of the orb, to find the player transform and set up everything.

ShouldFollow: Calculate distance between orb and player. 

FollowPlayer: Here lies the interesting part. For it to work, I make a few calculations to set the distance and angular velocity that the orb must asume each frame to emulate a gravitating motion. After a couple of trying and failing, I found out a bug where the orb would endlessly orbit around the player, and so I fixed this forcing the orb to travel on a straight line once too close to the player. It works flawlessly now.

CanGrapPickup: Make sure the player is able to consume the orb.


