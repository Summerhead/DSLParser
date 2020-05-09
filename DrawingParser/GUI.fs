module GUI

open System.Drawing
open System.Windows.Forms

open Parser


let mutable tasks = []
let setTasks (sendTasks : string list) = tasks <- sendTasks @ tasks;

let getForm () : Form =

    let createPicture (task : string) (e : PaintEventArgs) =
        printfn "Question: %s" task
        printfn "Result: %A" (result task)

        let ans = (Option.toArray <| (result task)).[0]
        let pairwise = Seq.pairwise (seq {for i in ans -> i})

        let mutable penCurrent : Pen = new Pen(Color.Red)
        for pair in pairwise do 
            printfn "pair: %A" pair
            let firstPoint, secondPoint = pair
            let (x1, y1), (x2, y2) = firstPoint, secondPoint
            e.Graphics.DrawLine(penCurrent, x1, y1, x2, y2)

    let form = new Form(MaximizeBox = true, Text = "Paint", Size = new Size(400, 400))
    let mainPanel = new Panel(Size = new Size(form.Width, form.Height))

    let picturePanelLocation = new Point(50, 10)

    let picturePanelList : Control list = 
        [for task in tasks do 
            let panel = new Panel(Size = new Size(form.Width, form.Height - 120), Location = picturePanelLocation)
            panel.Paint.Add (createPicture task)
            yield panel]

    let hideControl (control:Control) =
        control.Visible <- false
        control

    let defaultSize = new Size(70, 30)
    let buttonPrev = new Button(Text = "Prev", Size = defaultSize, Top = 300, Left = 10)
    let buttonNext = new Button(Text = "Next", Size = defaultSize, Top = 300, Left = 300)

    let switchPanel (panelList : Control list) (prevNextButtonList : Control list) (passedButton : Control) =
        let indexedControlArray =  List.mapi (fun index control -> index, control) panelList

        let displayedPanelPanelIndexPair = (List.filter (fun (_, control : Control) -> control.Visible = true) indexedControlArray).[0]
        let displayedPanelIndex, displayedPanel = displayedPanelPanelIndexPair

        let panelIndexToDisplay =
            match passedButton.Text with
            | "Prev" -> displayedPanelIndex - 1
            | "Next" -> displayedPanelIndex + 1
            | _ -> failwith("Unknown button")

        (snd indexedControlArray.[panelIndexToDisplay]).Visible <- true
        displayedPanel.Visible <- false

        printfn "displayedPanelIndex: %i; panelList.Length: %i" displayedPanelIndex panelList.Length
        if (displayedPanelIndex = 1 || displayedPanelIndex = panelList.Length) then
            passedButton.Visible <- false
            List.iter (fun (button1 : Control) -> if button1 <> passedButton then button1.Visible <- true) prevNextButtonList
        else 
            List.iter (fun (button1 : Control) -> if button1.Visible = false then button1.Visible <- true) prevNextButtonList

    let prevNextButtonList : Control list = [buttonPrev; buttonNext]
    List.iter (fun (button : Control) -> button.Click.Add (fun _ -> switchPanel picturePanelList prevNextButtonList button)) (prevNextButtonList)

    let flat2Darray (array2D : Control list list) : Control array = 
        seq { for x in [0..(List.length array2D) - 1] do 
                  for y in [0..(List.length array2D.[x]) - 1] do 
                      yield array2D.[x].[y] }
        |> Seq.toArray

    let controls : Control list list = [picturePanelList; prevNextButtonList]
    controls
    |> List.mapi (fun index array -> 
        match index with
        | 0 ->
            List.mapi (fun index panel -> 
                match index with
                | 0 -> panel
                | _ -> hideControl panel) array
        | _ ->
            List.mapi (fun index button -> 
                match index with
                | 0 -> hideControl button
                | _ -> button) array
        )
    |> flat2Darray
    |> mainPanel.Controls.AddRange

    mainPanel.Controls.Add buttonPrev
    mainPanel.Controls.Add buttonNext

    form.Controls.Add mainPanel

    form


let run () =
    let form = getForm()
    do Application.Run form

