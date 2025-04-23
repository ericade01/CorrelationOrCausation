from fastapi import FastAPI
from tradingview_ta import TA_Handler, Interval, Exchange
import uvicorn

app = FastAPI()

@app.get("/price")
def get_price(symbol: str = "BULL", exchange: str = "NASDAQ"):
    handler = TA_Handler(
        symbol=symbol,
        screener="america",
        exchange=exchange,
        interval=Interval.INTERVAL_1_MINUTE
    )
    analysis = handler.get_analysis()
    return {
        "symbol": symbol,
        "price": analysis.indicators["close"],
        "summary": analysis.summary
    }

if __name__ == "__main__":
    uvicorn.run("tv_server:app", host="0.0.0.0", port=8000)
