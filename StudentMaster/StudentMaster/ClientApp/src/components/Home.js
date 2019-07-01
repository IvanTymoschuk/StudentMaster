import React from 'react';
import { connect } from 'react-redux';

const Home = props => (
  <div>
    <h1>Hello, students!</h1>
    <h4>This is web service which notify future students about their studying date</h4>
    <br></br>
    <h3>So, JOIN US!</h3>

  </div>
);

export default connect()(Home);
