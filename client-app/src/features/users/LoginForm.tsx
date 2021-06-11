import { Form, Formik } from "formik";
import { values } from "mobx";
import React from "react";

export default function LoginForm() {
    return (
        <Formik 
            initialValues={{email: '', password: ''}}
            onSubmit={values => console.log(values)}
            >
                {({handleSubmit}) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        
                    </Form>
                )}
            </Formik>
    )
}