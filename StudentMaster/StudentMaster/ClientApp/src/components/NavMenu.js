import React from 'react';
import { Link } from 'react-router-dom';
import { connect } from "react-redux";
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';
import PropTypes from 'prop-types';
import { withRouter } from 'react-router-dom';
import { logout } from '../actions/authActions';

class NavMenu extends React.Component {
  state = {}
  logout(e) {
    e.preventDefault();
    this.props.logout();
    this.props.history.push('/');

  }
  render() {
    const { isAuthenticated, user } = this.props.auth;

    const logoutLink = (
      <NavItem onClick={this.logout.bind(this)}>
        <Glyphicon glyph="log-out" />Logout
  </NavItem>
    );

    const userLinks = (
      <LinkContainer to={"/profile"} activeClassName=""  >
        <NavItem>
          <Glyphicon glyph="file" />My profile
        </NavItem>
      </LinkContainer>
    );

    const reglinks = (
      <LinkContainer to={"/registration"} activeClassName="">
        <NavItem>
          <Glyphicon glyph="share" />Sign up
          </NavItem>
      </LinkContainer>
    );
    const loginLinks = (

      <LinkContainer to={"/login"} activeClassName="">
        <NavItem>
          <Glyphicon glyph="log-in" />Sign in
          </NavItem>
      </LinkContainer>

    );
    return (
      <Navbar collapseOnSelect expand="lg" fluid inverse bg="dark" variant="dark">
        <Navbar.Brand>
          <Link to={"/"}>Student Master</Link>
        </Navbar.Brand>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">

          <Nav >
            <LinkContainer to={"/about"} activeClassName="">
              <NavItem>
                <Glyphicon glyph="info-sign" />About
          </NavItem>
            </LinkContainer>

            {isAuthenticated ? <LinkContainer to={"/schedule"} activeClassName="">
              <NavItem>
                <Glyphicon glyph="list" />Schedule
          </NavItem>
            </LinkContainer> : ''}

            {isAuthenticated && user.roles === 'user' ? <LinkContainer to={'/pickdate'} activeClassName=''>
              <NavItem>
                <Glyphicon glyph='book' /> Studying
            </NavItem>
            </LinkContainer> : ''}

            {isAuthenticated && user.roles === 'admin' ? <LinkContainer to={'/admin'} activeClassName=''>
              <NavItem>
                <Glyphicon glyph='cog' /> Admin
              </NavItem>
            </LinkContainer> : ''}
          </Nav>

          <Nav className="navbar-right">
            {isAuthenticated ? userLinks : reglinks}
            {isAuthenticated ? logoutLink : loginLinks}
          </Nav>

        </Navbar.Collapse>
      </Navbar>
    );
  }
}
NavMenu.propTypes =
  {
    logout: PropTypes.func.isRequired
  }

const mapStateToProps = (state) => {
  return {
    auth: state.auth
  };
}

export default withRouter(connect(mapStateToProps, { logout })(NavMenu));

