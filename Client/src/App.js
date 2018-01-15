import React, { Component } from 'react';
import Car from './media/car';
import './App.css';

class App extends Component {
  render() {
    return (
      <div className="App">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 800 800">
          <Car color="pink" size="40" x="0" y="-200" />
          <Car color="red" size="40" x="10" y="-100" />
          <Car color="green" size="40" x="20" y="0" />
          <Car color="blue" size="40" x="30" y="100" />
          <Car color="orange" size="40" x="50" y="200" />
        </svg>
      </div>
    );
  }
}

export default App;
