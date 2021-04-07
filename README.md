Prototype 2: Momma Cub!

This is a management and training simulator with a virtual economy, that also tries to make playfulness out of programming concepts to get things done.

# Main points to redress from the previous playtest.

### Clarity

This was a main point raised -- it wasn't certain how cubs were trained at the Training Center. It was also mentioned that there were many steps and it was complex to the live tester. Step-by-step instructions and clear indicators are a priority.

### Training Process

The goals of the training process were not clear. Why do we have to drag cubs from one pen to the other? What do we gain from it? What do we lose?
It was also mentioned that feedbacks were not entirely distinguishable from dropping cubs to when they are trained (feedback for training occurring).

### Art

It was mentioned that the art could be more consistent between the main menu and the game.

### Lack of challenges, rewards

There is no clear game loop for cub training right now. It just happens automatically. (It's not a clicker game though). Adding clear obstacles to make it challenging and rewards for doing an activity in-game is a needed set of features.

# Issues playtesters

Tutorial feedback:

- Playtesters reported some errors with the conversation continue button. I only realized after that there was a secret canvas and background panel disallowing the raycast on the button.
- Two others reported that the tutorial may have been too lengthy, something that I suspected.
- Two people suggested that they wanted to learn as they go.
- The instructions themselves were clear

That said, the sample size is extremely small. I could ask 6 other people and they could answer differently, so it's impossible to tell for sure, other than that it should at least feel like a thinner tutorial.

On a side note, it seems some of my questions were a bit loaded and potentially could lead answers on. There is also the observation that some comments follow on previous' comments, so we may lose a bit of the individual response. A different questioning format may be more advantageous to the developer (e.g., where we can't see other people's comments).

Portrait: It seemed that the tutorial girl npc was a friendly figure which was intended.

# Live playtest

The liver tester commented cubs were replaced into anime girls which wasn't completely my attention (I just searched a stylized asset (anime character) in the asset store and grabbed the first one). I didn't have other models so I duplicated her. The cubs themselves are still in the game but only appear later on. Other than that, he suggested that I make highlight effects and things to see clearly which building is being interacted on. In short, visual feedback. After that, he ran into issues with the continue button not continuing at the right time after an issue. I noted that this was a bad thing to occur and should do something about it. On my end I do get it sometimes, but sometimes not. Basically, it's not perfectly reliable so I need to check a few things. After that he commented that it seemed to be a real game, which was intended. He also said that we don't know what the commands are in specific, of course this is true. I will take more time to make it better.

# Reflections

I did not get a lot of time out of the playtesting session or a lot of playtester feedback issues at the time I'm writing this, so my reflections are based on a very limited observation set. It seemed that most problems arose out of technical issues. It was a really bad timing for the prototyping week in terms of concurrent workload, so a lot of my work arose in the days after last Wednesday.

However, I feel good that I wrote my conversation system and my command line system to a point where it works as I intended but not completely reliably (which is normal for the time I had). I will recuperate these systems and make tools for future projects and iterations, and I got a lot of programming experience which was one of my main goals with this project.

In terms of addressing feedbacks for prototype 2 in this iteration, I addressed these issues of clarity but in terms of step-by-step instructions. It makes sense, but I think that as some playtesters reported in the issues, the dragging cubs stuff was a bit more intuitive. I knew this would be reported in advance, but I had to explore this venue about the input field. I feel somewhat happy with the decision, although it hasn't come out in a polished way yet! I believe there is potential in it, as most playtesters reported. I don't want to explode the scope either, so I kept it simple : you can have commands to create tasks, dispatch them to workers, have them rest when out of stamina, feed cubs, and train cubs. The rest is just statistics -- workers' work generate food, which allows cubs to eat, and sated cubs can be trained efficiently. Cubs are sold for coins. Coins are spent to buy cubs and workers, and upgrades. Workers can be assigned long or short tasks, and this influences the reward at the end of each batch processing and the final reward. It influences stamina consumption, and workers need stamina to work, so they rest. There may be challenges during work in the form of work events. The satisfaction is to manage multiple workers at the same time efficiently (find a good way to progress is the reward). All these were mentioned in the tutorial or should be made clearly.

END OF PLAYTESTING NOTES AND REfLECTIONS

# This is a management and training simulator with a virtual economy, that also tries to make playfulness out of programming concepts to get things done.

# Iteration 1 Design Journal

I've decided to address the issues of clarity raised in playtesters' feedback.
I mainly added tutorials for step-by-step instructions and revised it for clarity.
The player now starts at a logical point in the game: where they have to first buy a cub to see what it is they're playing with mainly.
The player starts with a single cub as suggested in the feedbacks. Then I walk the player step-by-step.
I've attempted to make the mini-game at the training centre clearer and refine its workings. Edit: The last concern ended up changing a lot of the design itself, as I felt that dragging cubs around wasn't the right direction. I think I would relay the satisfaction of dragging things to a more minor part of the game.

# Iteration 1 details

I had to find something I personally enjoyed (both as a developer and gamer). I've always had the lingo desire, so as I was struggling to correct or revise many of the problems related to the dragging mechanism, and especially how to make it worthwhile in a coherent gameplay, I realized that I didn't actually care about that level of detail for the things I wanted for the game. The moment-to-moment design is something of a weakness for me. But also I wanted to test out ideas that took the playtesters' feedback into consideration. For example, some reported that they wouldn't design gameplay around dragging the cubs (and some said they loved it). I realized that I didn't actually know why I intended to do that in the first place, but that level of abstraction was merely to "get things done" so that I could check out management related gameplay. 

What I got out of this last iteration was simply that I wanted to make mini-games but in a way that would connect to another level of abstraction in the business management "pipeline" : that is managing workers who would manage your assets. Right now, managing directly your cubs feels more like you're a farmer, so an independent worker. However, one of my main interests was to create an ambience where you're building this company with progressively non-materialistic goals. That is, to create some kind of 'family' warm ambience that would create this sense of zany in the player who is trying to make profits initially (while this is necessary, it would be redirected towards a different goal). 

The dissonance is what I'm after. Obviously, this scope would require a lot of time, so the main challenge for the final deadline is to make it simple enough with those ideas, so that I'm happy with the result. At a minimum, the MVP would include the ideas that I would enjoy playing myself the most : managing some kind of economy or business as some kind of coach, while retaining the satisfaction of building satisfying relationships (...with NPCs). Reference comparisons would be a mix of Fire Emblem: Three Houses, and Harvest Moon. So basically, a very boiled down version of these would be like, you can operate on workers who would operate on your cubs. This means, you could write commands like 'Analiese.work(fields, 8)' meaning that worker would go on to work the 'fields' for 8 hours in-game. Working generates food resources, which you can program the workers to feed the cubs. (This becomes like a natural observation... : feed() to feed all the cubs. Ideally you would have these commands somewhere handy, (a fake API), and then you'd have more interesting commmands like, 'repeat Analiese.work(fields, 8)' to repeat that action every 8 hours. I would restrict the range of commands to very basic opcodes related to the core gameplay, because I only have 2 weeks.

The second main interest was to make some kind of educative 'coding' game out of it. It wouldn't be a straight up real educative thing as I'm not an expert, but it would playfully make use of programming concepts along the way. Making a story or game out of these concepts? Overall, an accessible, playful educative game that would have what I find satisfying in it: writing high level commands/scripts. The idea came from attending a Python workshop where I was impressed by the host showing us an extremely high level of doing things in ML : "train(...args)". That was almost it. That would almost do everything. Geez! It left an impression in my soul. 

Now hopefully I can do something that I can be happy with within the next two weeks : otherwise I don't feel so bad in pursuing the project further (I know this is code for I'll never do it, but I just like these elements personally). Basically when Joachim mentioned LUA (Love2D) and high level scripts, I thought, yeah there's something right in that level of abstraction, for this kind of game actions. I was curious about it, but I have to admit it was also a lot to do with what I don't know how to do currently in Unity. Making it more command-based is both a necessity and an interest.

My iterative questions are in the issues, and they are mostly about how people felt about the tutorials and fake simplified cli. Basically a lot of these changes are directly from the playtesting : people felt it was complicated and hard to understand so I made step by step instructions, with a customized way of pointing out the details that are important at every step (e.g., making a text label appear at the same time as an instruction from the Tutorial Girl that would make use of that was labelled by it, or making a building only clickable when the tutorial instruction says 'Click on the Training Centre now'.). To do that I started working on a customized conversation action system that would just have the stuffs I need (it parses what kind of instruction I want to give it. Other frameworks were too complicated to do what I needed only) on top of the CLI stuff. But now, it's all about making it to the finish line with a completed project. Another inspiration was simply the many code examples in object-oriented programming classes where westudy inheritance and polymorphism with farm animals and bank accounts but it's never applied in a meaningful way right now. The experience of living and making those academic instructions work in a real-time environment was an interesting idea to me.

# 1-week prototyping

Playtesters: Game prototype was frozen around 11:20 PM (last hotfix pushed) on February 24th. The versions before are obsolete.

The interest in this project was to create a hypercasual lighthearted themed game.
