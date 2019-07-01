import React from 'react';
import PropTypes, {
    number
} from 'prop-types';
import {
    connect
} from "react-redux";
import {
    studyInfo
} from "../../actions/accActions";
import Moment from 'react-moment';

class StudyInfo extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            studyDate: '',
            tillEnd: number
        }

    }
    componentWillMount() {
        const {
            user
        } = this.props.auth;
        let id = user.id;
        this.props.studyInfo({
            id
        }).then((res) => {

            this.setState({
                studyDate: res.data.studyDate,
                tillEnd: res.data.tillEnd
            })
        })
    }
    render() {
        const { studyDate, tillEnd } = this.state;
        const tillEndPositiveScenario =
            (
                <div>
                    <h1 className="header">Till start of your studying left: {tillEnd}</h1>
                    <h4 className="header">Date: <Moment format="YYYY/MM/DD">{studyDate}</Moment></h4>
                </div>
            );
        return (<div>
            {tillEnd > 0 ? tillEndPositiveScenario : ''}
            {tillEnd === 0 ? <h1>Your studying starts today</h1> : ''}
            {tillEnd < 0 ? <h1>You didn`t choose any study date</h1> : ''}
        </div>
        );
    }
}
StudyInfo.propTypes = {
    studyInfo: PropTypes.func.isRequired
}
const mapStateToProps = (state) => {
    return {
        auth: state.auth
    };
}

export default connect(mapStateToProps, {
    studyInfo
})(StudyInfo);