import { HttpClient } from 'aurelia-fetch-client';
import { inject, computedFrom } from 'aurelia-framework';
import { State } from 'aurelia-route-recognizer';
import { observable } from 'aurelia-framework';

@inject(HttpClient)
export class Game {
    
    private isLoaded: boolean = false;    
    private isBlacksTurn: boolean = false;
    private isShootMode: boolean = false;

    private board: Board = new Board([[0,1],[2,0]]);

    @observable originalAmazonPosition?: Position;

    @observable selectedAmazon?: Position;
    
    constructor(http: HttpClient) {

        http.fetch('/api/games/1')

            .then(result =>
                result.json() as Promise<GameModel>)
            .then(data => {

                this.isShootMode = false;
                this.isBlacksTurn = data.isBlacksTurn
                this.board = new Board(data.board)
                this.isLoaded = true;
            })

            .catch(err => {
                console.log(err)
            });
    }

    public clicked(x: number, y: number) {
        this.positionClicked(new Position(x,y));
    }

    public positionClicked(position: Position) {
        
        const mouse = { ... position, ... this.board.cell(position) }
        console.log("clicked: ", this.isShootMode, this.isBlacksTurn, mouse.piece , !! this.selectedAmazon)
        
        if (!this.isShootMode && !this.selectedAmazon && mouse.piece == (this.isBlacksTurn ? Piece.Black : Piece.White)) {
            // selecting or unselecting an amazon
            const color = this.isBlacksTurn ? "black" : "white";
            if (position.equals(this.selectedAmazon)) {
                console.log(`un-selected ${color} amazon: ${position}`)
                this.selectedAmazon = undefined;
            } else {
                console.log(`selected ${color} amazon: ${position}`)
                this.selectedAmazon = position;
            }
        } else if (!this.isShootMode && this.selectedAmazon && mouse.isHighlighted) {
            // moving an amazon and switching to shoot-mode
            const original = this.selectedAmazon;
            this.selectedAmazon = undefined; // also clears all selections and highlights            
            
            this.board.cell(original).piece = Piece.None;
            this.board.cell(position).piece = this.isBlacksTurn ? Piece.Black : Piece.White;
            this.selectedAmazon = position;
            
            this.originalAmazonPosition = original
            this.isShootMode = true;
            console.log(`moving ${original} to ${position}`)
        } else if (!this.isShootMode && mouse.piece == Piece.None && !mouse.isHighlighted) {
            // unselecting by clicking on empty
            this.selectedAmazon = undefined;
        } else if (this.isShootMode  && mouse.isHighlighted) {
            // shooting to a highlighted cell;  !! this.selectedAmazon is implied
            this.isShootMode = false;
            this.selectedAmazon = undefined // also clears all selections and highlights
            this.board.cell(position).piece = Piece.Arrow;
            this.isBlacksTurn = !this.isBlacksTurn;
        } else if (this.isShootMode && this.originalAmazonPosition && mouse.piece == Piece.None && this.selectedAmazon) {
            // cancel in shoot mode
            const original = this.originalAmazonPosition;
            const selected = this.selectedAmazon;
            this.originalAmazonPosition = undefined;
            this.selectedAmazon = undefined; // also clears all selections and highlights            
            
            this.board.cell(original).piece = this.isBlacksTurn ? Piece.Black : Piece.White;
            this.board.cell(selected).piece = Piece.None;
            this.isShootMode = false;
        } else {
            var snd = new Audio("data:audio/wav;base64,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qJrx4KOQjahZxWbcZgztj2c49nKmkId44S71j0c8eV9yDK6uPRzx5X18eDvjvQ6yKo9ZSS6l//8elePK/Lf//IInrOF/FvDoADYAGBMGb7FtErm5MXMlmPAJQVgWta7Zx2go+8xJ0UiCb8LHHdftWyLJE0QIAIsI+UbXu67dZMjmgDGCGl1H+vpF4NSDckSIkk7Vd+sxEhBQMRU8j/12UIRhzSaUdQ+rQU5kGeFxm+hb1oh6pWWmv3uvmReDl0UnvtapVaIzo1jZbf/pD6ElLqSX+rUmOQNpJFa/r+sa4e/pBlAABoAAAAA3CUgShLdGIxsY7AUABPRrgCABdDuQ5GC7DqPQCgbbJUAoRSUj+NIEig0YfyWUho1VBBBA//uQZB4ABZx5zfMakeAAAAmwAAAAF5F3P0w9GtAAACfAAAAAwLhMDmAYWMgVEG1U0FIGCBgXBXAtfMH10000EEEEEECUBYln03TTTdNBDZopopYvrTTdNa325mImNg3TTPV9q3pmY0xoO6bv3r00y+IDGid/9aaaZTGMuj9mpu9Mpio1dXrr5HERTZSmqU36A3CumzN/9Robv/Xx4v9ijkSRSNLQhAWumap82WRSBUqXStV/YcS+XVLnSS+WLDroqArFkMEsAS+eWmrUzrO0oEmE40RlMZ5+ODIkAyKAGUwZ3mVKmcamcJnMW26MRPgUw6j+LkhyHGVGYjSUUKNpuJUQoOIAyDvEyG8S5yfK6dhZc0Tx1KI/gviKL6qvvFs1+bWtaz58uUNnryq6kt5RzOCkPWlVqVX2a/EEBUdU1KrXLf40GoiiFXK///qpoiDXrOgqDR38JB0bw7SoL+ZB9o1RCkQjQ2CBYZKd/+VJxZRRZlqSkKiws0WFxUyCwsKiMy7hUVFhIaCrNQsKkTIsLivwKKigsj8XYlwt/WKi2N4d//uQRCSAAjURNIHpMZBGYiaQPSYyAAABLAAAAAAAACWAAAAApUF/Mg+0aohSIRobBAsMlO//Kk4soosy1JSFRYWaLC4qZBYWFRGZdwqKiwkNBVmoWFSJkWFxX4FFRQWR+LsS4W/rFRb/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////VEFHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU291bmRib3kuZGUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAwNGh0dHA6Ly93d3cuc291bmRib3kuZGUAAAAAAAAAACU=");  
            snd.play();
        }
    }

    public selectedAmazonChanged(newValue: Position, oldValue: Position) {
        if (oldValue) {
            this.board.cell(oldValue).isSelected = false;
            console.log(`cleared isSelected for ${oldValue.x} ${oldValue.y}`)

            // clear all highlights
            for (let pos of this.board.all()) {
                this.board.cell(pos).isHighlighted = false;
            }
        }
        
        if (newValue) {
            this.board.cell(newValue).isSelected = true;
            console.log(`set isSelected for ${newValue.x} ${newValue.y}`)

            // highlight reachable squares
            for(let pos of this.board.reachable(newValue)) {
                this.board.cell(pos).isHighlighted = true;
            }            
        }
    }
}

interface GameModel {
    isBlacksTurn: boolean;
    board: number[][];   
}

export class Board {
    
    public readonly cells: Square[][];
    public readonly width : number;
    public readonly height : number;

    constructor(board: number[][])
    {
        this.cells = board.map(row => row.map(i => Square.create(i)))
        this.width = board[0].length
        this.height = board.length
    }

    public cell(pos: Position): Square 
    {
         return this.cells[pos.y][pos.x]
    }
 
    public isInside(pos: Position): boolean {        
        return 0 <= pos.x && pos.x < this.width &&
            0 <= pos.y && pos.y < this.height;
    }

    public *all() : Iterable<Position> {
        for (let y = 0; y < this.height; y++) {            
            for (let x = 0; x < this.width; x++) {      
                yield new Position(x, y);
            }
        }
    }

    public *range() : Iterable<number>
    {
        var n = Math.max(this.width, this.height);             
        for (let i = 1; i < n; i++) { 
            yield i;
        }              
    }  
    
    public * reachable(selected: Position) : Iterable<Position>
    {
        for (let dir of Directions.all) {
            for (let distance of this.range()) {
                const pos = selected.transpose(dir, distance)
                if (!this.isInside(pos)) {
                    break; // stop the distance loop at edge of board
                }
                if (this.cell(pos).piece != Piece.None) {
                    break; // stop the distance loop if a piece is blocking the way
                }
                // result.push(pos);            
                yield pos;
            }
        }      
    }
}

export class Square {
    public isSelected: boolean = false;
    public isHighlighted: boolean = false;
    public piece: Piece = Piece.None;

    constructor(piece: Piece) {
        this.piece = piece;
    }

    static create(n: Number): Square {
        return new Square(n == 1 ? Piece.White : n == 2 ? Piece.Black : n == 3 ? Piece.Arrow : Piece.None);
    }
}

export class Position {
    
    constructor(readonly x: number, readonly y: number) {
    }

    equals(other: Position | undefined) {
        return other && other.x == this.x && other.y == this.y;
    }

    transpose(dir: Direction, n: number): Position {
        return new Position(this.x + dir.dx * n, this.y + dir.dy * n)
    }
}

class Direction {
    constructor(readonly dx: number, readonly dy: number) {
    }
}

class Directions {
    static readonly N: Direction = new Direction(0, -1);
    static readonly S: Direction = new Direction(0, 1);
    static readonly W: Direction = new Direction(-1, 0);
    static readonly E: Direction = new Direction(1, 0);
    static readonly NE: Direction = new Direction(1, -1);
    static readonly SW: Direction = new Direction(-1, 1);
    static readonly NW: Direction = new Direction(-1, -1);
    static readonly SE: Direction = new Direction(1, 1);

    static readonly all: Direction[] = [
        Directions.N, Directions.E, Directions.S, Directions.W,
        Directions.NE, Directions.SE, Directions.SW, Directions.NW
    ];
}


enum Piece {
    None = 0, White, Black, Arrow
}

