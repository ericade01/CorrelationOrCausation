import yfinance as yf
import pandas as pd
import sys

def download_data(ticker_symbol, period, interval):
    try:
        data = yf.download(
            tickers=ticker_symbol,
            period=period,
            interval=interval,
            prepost=True,
            threads=True
        )

        if data.empty:
            print("ERROR: No data downloaded.")
            return

        data.reset_index(inplace=True)
        output_filename = f"{ticker_symbol.lower()}_{interval}.csv"
        data.to_csv(output_filename, index=False)
        print(f"SUCCESS: {output_filename}")
    except Exception as e:
        print(f"ERROR: {str(e)}")

if __name__ == "__main__":
    if len(sys.argv) != 4:
        print("ERROR: Usage is python ypy.py <TICKER> <PERIOD> <INTERVAL>")
    else:
        ticker = sys.argv[1]
        period = sys.argv[2]
        interval = sys.argv[3]
        download_data(ticker, period, interval)
