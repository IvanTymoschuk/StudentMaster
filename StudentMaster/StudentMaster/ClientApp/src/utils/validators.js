export function isValidPassword(pass) {

    return !/^(?=.*\d)(?=.*[a-zA-Z]).{6,20}$/.test(pass);
}

export function isValidEmail(email) {

    return !/.+@.+\.[A-Za-z]+$/.test(email);
}