```mermaid
sequenceDiagram
    participant Exchange
    participant Pricing Engine
    participant Data Store

    Exchange ->> Pricing Engine: post transaction
    Pricing Engine ->> Pricing Engine: validate transaction
    Pricing Engine ->> Data Store: store transaction
    Data Store ->> Pricing Engine: aggregate transactions by price
    Pricing Engine ->> Pricing Engine: compute average price
    Pricing Engine ->> Pricing Engine: convert price to stock price
    Pricing Engine ->> Data store: store stock price
```