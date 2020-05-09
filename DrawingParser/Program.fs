open System

open GUI


[<EntryPoint>]
let main _ =
    let tasks = ["set 80 80; right 60, down 10, right 50, down 80, 
    left 30, up 30, left 10, down 30, left 30, up 40, left 10, 
    down 20, left 10, down 10, left 10, up 20, left 10, up 60"; 

    "set 80 80; right 10, down 10, right 20, up 10, 
    right 10, down 20, right 50, up 20, left 10, up 10, right 20, 
    down 90, left 10, up 20, left 10, down 20, left 10, up 30, 
    left 30, down 30, left 10, up 20, left 10, down 20, left 10, 
    up 40, left 10, up 40"; 

    "set 50 50; up 30, right 10, up 10, right 20, 
    up 10, right 40, down 10, right 10, down 10, right 10, down 10, 
    right 20, down 10, right 10, down 10, right 20, down 10, 
    right 10, down 10, right 10, down 20, right 10, 
    down 20, right 10, down 30, right 20, down 20, left 30, down 100, 
    left 20, up 50, left 10, up 40, left 10, up 30, left 10, down 20, 
    left 30, up 10, right 20, up 20, left 20, up 10, left 20, up 10, 
    left 10, up 10, left 10, up 20, left 10, up 20, left 10, up 20, 
    left 20, down 10, left 10"]

    setTasks tasks

    run()
    0 // return an integer exit code
