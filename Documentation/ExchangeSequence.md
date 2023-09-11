```mermaid
sequenceDiagram
    participant Client
    participant Exchange
    participant Data Store
    participant Pricing Engine

    Client ->> Exchange: post trade
    Exchange ->> Exchange: validate trade
    Exchange ->> Data Store: store trade
    Exchange ->> Exchange: convert trade to transaction
    Exchange ->> Pricing Engine: post transaction
```
