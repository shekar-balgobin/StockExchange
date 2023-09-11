```mermaid
flowchart
    t[Trades]
    es[Exchange Service]
    eds[Data Store]
    pes[Pricing Engine Service]
    peds[Data Store]
    s[Stocks]

    subgraph Broker
        t
        s
    end

    subgraph Exchange
        t--trade-->es
        es--trade-->eds
    end

    subgraph Prcing Engine
        es--transaction-->pes
        pes--transaction-->peds
        pes--stock price-->s
    end
```