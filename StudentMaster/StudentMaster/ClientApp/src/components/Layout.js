import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';
import './Layout.css';
export default props => (
  <Grid fluid>
    <Row>
      <NavMenu />
    </Row>
    <Row>
      <div className="container">
        {props.children}
      </div>
    </Row>
  </Grid>
);
