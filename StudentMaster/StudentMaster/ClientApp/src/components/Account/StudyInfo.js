import React, { Component } from 'react';
import PropTypes, { number } from 'prop-types';
import { connect } from "react-redux";
import { studyInfo } from "../../actions/accActions";

class StudyInfo extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            studyDate: '',
            tillEnd: number
        }

    }
componentWillMount(){
    const { user } = this.props.auth;
    let id = user.id;
    this.props.studyInfo({
        id
    }).then((res)=>{
       
            this.setState({studyDate:res.data.studyDate,tillEnd:res.data.tillEnd})
        })
}
render() {
    const { studyDate,tillEnd} = this.state;
    return (<div>
        <h1 className="header">Till start of studying left: {tillEnd}</h1>
        <h5 className="header">Date: {studyDate}</h5></div>);
}
}
    StudyInfo.propTypes =
    {
        studyInfo: PropTypes.func.isRequired
    }
const mapStateToProps = (state) => {
    return {
        auth: state.auth
    };
}

export default connect(mapStateToProps, { studyInfo })(StudyInfo);