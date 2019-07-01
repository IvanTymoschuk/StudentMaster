import React, {
} from 'react';
import DatePicker from 'react-date-picker';
import PropTypes from 'prop-types';
import {
    connect
} from "react-redux";
import {
    pickDate
} from "../../actions/accActions";
import classnames from 'classnames';
import './PickDatePage.css';
import {
    Panel
} from 'react-bootstrap'
class PickDatePage extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            studyDate: new Date(),
            errors: {}
        }
        this.onChange = this.onChange.bind(this);

    }
    setStateByErrors = (name, value) => {
        if (!!this.state.errors[name]) {
            let errors = Object.assign({}, this.state.errors);
            delete errors[name];
            this.setState({
                [name]: value,
                errors
            })
        } else {
            this.setState({
                [name]: value
            })
        }
    }
    onChange(studyDate) {
        this.setState({
            studyDate
        })
    }

    onSubmitForm = (e) => {
        e.preventDefault();
        const {
            user
        } = this.props.auth;
        let id = user.id;
        const {
            studyDate
        } = this.state;
        this.props.pickDate({
            userId: id,
            studyDate
        }).then(() => {
            this.props.history.push("/schedule");
        },
            (err) => {
                this.setState({
                    errors: err.response.data,
                    isLoading: false
                });
            });
    }
    render() {
        const {
            errors
        } = this.state;
        return (

            <div className="mycontain">

                <form className="myform" onSubmit={this.onSubmitForm}>
                    <Panel>
                        <h2 className="header text-center">Pick your study date</h2>
                        <div className={classnames('form-group', { 'has-error': !!errors.StudyDate })}>
                            <DatePicker
                                className="date"
                                minDate={new Date()}
                                onChange={this.onChange}
                                value={this.state.studyDate}
                            />
                            {!!errors.StudyDate ? <span className="help-block">{errors.StudyDate}</span> : ''}
                        </div>
                        {
                            !!errors.invalid ?
                                <div className="alert alert-danger">
                                    <strong>Danger!</strong> {errors.invalid}.
                        </div> : ''}

                        <button type="submit" className="btn btn-info" >Next</button>
                    </Panel>

                </form>
            </div>
        );

    }
}

PickDatePage.propTypes = {
    pickDate: PropTypes.func.isRequired
}
const mapStateToProps = (state) => {
    return {
        auth: state.auth
    };
}

export default connect(mapStateToProps, {
    pickDate
})(PickDatePage);