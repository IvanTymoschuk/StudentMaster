import React from 'react';
import { LinkContainer } from "react-router-bootstrap";
import LoginForm from './LoginForm';
import { Col, Row, Panel } from 'react-bootstrap';

class LoginPage extends React.Component {
    render() {
        return (
            <Row>
                <Col md={4} mdOffset={4}>
                    <Panel>
                        <LoginForm />
                        <LinkContainer to="/forgotpassword" >
                            <div className="text-center" >
                                <a className="mt-2">Forgot Password?</a>
                            </div>
                        </LinkContainer>
                    </Panel>
                </Col>
            </Row>
        );
    }
}


export default LoginPage;