using System;
using System.Drawing;
using System.Windows.Forms;

public class FiftenPuzzle : Form
{
    Tile[,] nums = new Tile[4, 4];
    Point emptyTile = new Point(3, 3);
    
    public FiftenPuzzle() : base() 
    {
        GenerateTiles();
        RandomlyClickTiles();
    }

    private void GenerateTiles() 
    {
        int count = 0;
        for (int y = 0; y < 4; y++) 
        {
            for(int x = 0; x < 4 && count != 15; x++)
            {
                String imgName = "Pictures/" + count + ".png";
                Image img = Image.FromFile(imgName);
                img = resizeImage(img, new Size(200, 200));
                nums[y,x] = new Tile(count++, x, y);
                nums[y,x].Image = img;
                nums[y,x].Height = img.Height;
                nums[y,x].Width = img.Width;
                nums[y,x].BorderStyle = BorderStyle.None;
                nums[y,x].Size = new Size(200, 200);
                this.Controls.Add(nums[y,x]);
                nums[y, x].Location = new Point(x * 200, y * 200);
                nums[y, x].SetPosition(new Point(x, y));
                nums[y,x].MouseClick += Puzzle_MouseClick;
            }
        }
        DetectAndSetMovableTiles();
    }

    private void DetectAndSetMovableTiles()
    {
        Point[] movables = FindTilesThatCanMove();
        Point currentPoint;
        int count = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4 && count++ != 15; x++)
            {
                currentPoint = nums[y, x].GetPosition();
                nums[y, x].SetUp(false);
                nums[y, x].SetDown(false);
                nums[y, x].SetLeft(false);
                nums[y, x].SetRight(false);
                if(currentPoint == movables[0]) //check up
                {
                    nums[y, x].SetUp(true);
                }
                else if (currentPoint == movables[1]) //check down
                {
                    nums[y, x].SetDown(true);
                }
                else if (currentPoint == movables[2]) //check left
                {
                    nums[y, x].SetLeft(true);
                }
                else if (currentPoint == movables[3]) //check right
                {
                    nums[y, x].SetRight(true);
                }
            }
        }
    }

    private Point[] FindTilesThatCanMove()
    {
        int x = emptyTile.X,
            y = emptyTile.Y;
        Point[] points = new Point[4];
        Point tileAbove = new Point(-1, -1),
              tileUnder = new Point(-1, -1),
              tileLeft = new Point(-1, -1),
              tileRight = new Point(-1, -1);
        if (y > 0)
        {
            tileAbove = new Point(x, y - 1);
        }
        if (y < 3)
        {
            tileUnder = new Point(x, y + 1);
        }
        if (x > 0)
        {
            tileLeft = new Point(x - 1, y);
        }
        if (x < 3)
        {
            tileRight = new Point(x + 1, y);
        }
        points[0] = tileAbove;
        points[1] = tileUnder;
        points[2] = tileLeft;
        points[3] = tileRight;

        return points;
    }

    private Image resizeImage(Image imgToResize, Size size)
    {
        return (Image)(new Bitmap(imgToResize, size));
    }

    private void RandomlyClickTiles()
    {
        int counter = 500,
            randX,
            randY; 
        Random ranGen = new Random();
        while(counter-- > 0)
        {
            randX = ranGen.Next(0, 4);
            randY = ranGen.Next(0, 4);
            if(randX + randY != 6)
            {
                SwapEmptyAndTile(nums[randY, randX]);
            }
        }
    }

    void Puzzle_MouseClick(object sender, MouseEventArgs e)
    {
        Tile clickedTile = (Tile)sender;
        SwapEmptyAndTile(clickedTile);
        CheckWin();
    }

    private void SwapEmptyAndTile(Tile tile)
    {
        if (tile.GetUp() ||
            tile.GetDown() ||
            tile.GetLeft() ||
            tile.GetRight())
        {
            int xIndex = tile.GetIndexX(),
                yIndex = tile.GetIndexY(),
                xPos = tile.GetPosition().X,
                yPos = tile.GetPosition().Y;
            nums[yIndex, xIndex].SetPosition(emptyTile);
            nums[yIndex, xIndex].Location = new Point(emptyTile.X * 200, emptyTile.Y * 200);
            emptyTile = new Point(xPos, yPos);
        }
        DetectAndSetMovableTiles();
    }

    private void CheckWin()
    {   
        bool checkWin = true;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if(nums[y,x].GetPosition() != new Point(x,y))
                {
                    checkWin = false;
                }
            }
        }
        if (checkWin)
        {
            MessageBox.Show("You Win!");
        }
    }

    public static void Main(string[] args) 
    {
        FiftenPuzzle game = new FiftenPuzzle();
        game.Size = new Size(817, 847);
        Application.Run(game);
    }
}

class Tile : PictureBox
{
    private Point position;
    private int tileNum,
                xIndex,
                yIndex;
    private bool canGoUp = false,
                 canGoDown = false,
                 canGoLeft = false,
                 canGoRight = false;

    public Tile(int name, int x, int y)
    {
        tileNum = name;
        xIndex = x;
        yIndex = y;
    }

    //GETTERS AND SETTERS
    public int GetIndexY(){
        return yIndex;
    }
    public int GetIndexX(){
        return xIndex;
    }
    public void SetIndexY(int y){
        yIndex = y;
    }
    public void SetIndexX(int x){
        xIndex = x;
    }
    public Point GetPosition(){
        return position;
    }
    public void SetPosition(Point newPosition){
        position = newPosition;
    }
    public int GetTileNum(){
        return tileNum;
    }
    public bool GetUp(){
        return canGoUp;
    }
    public bool GetDown(){
        return canGoDown;
    }
    public bool GetLeft(){
        return canGoLeft;
    }
    public bool GetRight(){
        return canGoRight;
    }
    public void SetUp(bool state){
        this.canGoUp = state;
    }
    public void SetDown(bool state){
        this.canGoDown = state;
    }
    public void SetLeft(bool state){
        this.canGoLeft = state;
    }
    public void SetRight(bool state){
        this.canGoRight = state;
    }
}