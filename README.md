# CSharp
Comments:  
  
UI:  
To open menu: G for right instrument (main hand) and H for left instrument (off hand)  
To select instrument: J to move left and K to move right  
To close menu: Same button as opening (G/H) or enter/return  
Julia will do tooltips for selected menu instruments  
  
Problems:  
Looks like shit - Leif will fix sprites  
Need more instruments (currently only have 2 total)  
Should we use lifebar or hearts? (hearts implemented as boxes but not displayed)  
  
Music:  
Currently 3 clips/streams: 1 is piano (always playing), 2 is violin (swappable) and 3 is drums (not playing initially)  
Stops when "fighting" in player controller is set to true (currently not implemented, change manually in inspector)  
To change, swap right instrument (G) from violin to harp. This will stop 2 (violin) and start 3 (drums)  
  
Problems:  
Music restarts after every swap (fix so it resumes from a certain point in some way)  
No crossfades (should be relatively easy fix)  
  
TODO:  
Need to decide what instruments we have/what they do  
Need to decide on final UI design  
Shit ton of content creation  
  
