import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';

export default props => (
  <Grid fluid>
    <Row>
       <NavMenu />
       </Row>
<Row>
  <div className ="container">
        {props.children}
        </div>
        </Row>
  </Grid>
);
