# Architecture

## Layered view

```mermaid
flowchart TB
    UI[WinForms UI\nForm1] --> APP[Application Layer\nCheckersSession]
    APP --> DOMAIN[Domain Layer\nCheckersGame]
    DOMAIN --> BOARD[CheckersBoard]
    DOMAIN --> MODEL[Value Objects\nPiece / Position / Move]
```

## Responsibilities

- `Form1`: input/output adapter. Handles click events and paints current presentation state.
- `CheckersSession`: UI use-case coordinator. Tracks selection, status text, and presentation model.
- `CheckersGame`: rule engine. Validates moves, enforces mandatory captures, multi-capture, promotion, and winner detection.
- `CheckersBoard`: board state storage and piece enumeration.
- `Piece`, `Position`, `Move`: immutable domain model types used across layers.

## Runtime interaction

```mermaid
sequenceDiagram
    participant User
    participant Form as Form1
    participant Session as CheckersSession
    participant Game as CheckersGame

    User->>Form: Click board cell
    Form->>Session: ClickCell(position)
    Session->>Game: GetLegalMoves / ApplyMove
    Game-->>Session: Updated game state
    Session-->>Form: BoardPresentation
    Form-->>User: Repaint board + status
```

## Extension points

- Rules variants: add strategy layer over `CheckersGame` move-generation methods.
- AI: add application service that evaluates `CheckersGame` legal moves and picks best move.
- Persistence: add board serialization/deserialization adapter in application layer.
- Multiplayer/network: replace local click source with command stream while keeping domain unchanged.
