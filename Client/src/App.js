import React, { Component } from 'react';
import Car from './media/car';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        <header className="App-header">
          <Car color="#ff0000" />
        </header>
      </div>
    );
  }
}

export default App;
