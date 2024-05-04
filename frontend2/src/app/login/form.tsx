"use client";

import { LockOutlined, UserOutlined, ExclamationCircleOutlined } from "@ant-design/icons";

import { Form, Input, Button, Col, Row, Typography } from "antd";
import { useState } from "react";
import { useFormState, useFormStatus } from "react-dom";
import { useRouter } from "next/navigation";

const initialValues = {
  username: "",
  password: "",
};

const initialState = {
  values: initialValues,
};

type Values = typeof initialValues;

interface FormState {
  values: Values;
}

const onFormStateChange = (prevState: FormState, formData: FormData): FormState => {
  const values: Values = {
    username: formData.get("username")?.toString() ?? "",
    password: formData.get("password")?.toString() ?? "",
  };

  return {
    values,
  };
};

export default function SignInForm() {
  const [state, action] = useFormState(onFormStateChange, initialState);
  const [formError, setFormError] = useState("");
  const router = useRouter();

  return (
    <Row justify="center" align="middle" className="min-h-screen">
      <Col span={8}>
        <Typography.Title level={2}>Sign in</Typography.Title>
        <Form
          name="normal_login"
          className="login-form"
          initialValues={state.values}
          action={action}
          onFinish={async (values) => {
            setFormError("");

            const response = await fetch("http://localhost:5002/auth/login", {
              method: "POST", // *GET, POST, PUT, DELETE, etc.
              mode: "cors", // no-cors, *cors, same-origin
              cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
              credentials: "same-origin", // include, *same-origin, omit
              headers: new Headers([["Content-Type", "application/json"]]),
              redirect: "follow", // manual, *follow, error
              referrerPolicy: "no-referrer", // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
              body: JSON.stringify(values), // body data type must match "Content-Type" header
            });

            if (!response.ok) {
              setFormError("Invalid username or password");
              return;
            }

            const result = await response.json();

            localStorage.setItem("user", JSON.stringify(result));
          }}
          onFinishFailed={() => console.log("PPC")}
        >
          <Form.Item
            name="username"
            rules={[
              {
                required: true,
                message: "please input your username!",
              },
            ]}
          >
            <Input
              prefix={<UserOutlined className="site-form-item-icon" />}
              placeholder="username"
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[
              {
                required: true,
                message: "please input your password!",
              },
            ]}
          >
            <Input
              prefix={<LockOutlined className="site-form-item-icon" />}
              type="password"
              placeholder="password"
            />
          </Form.Item>

          <Form.Item>
            <LoginButton />
            <div className="flex h-8 items-end space-x-1" aria-live="polite" aria-atomic="true">
              {formError && (
                <>
                  <ExclamationCircleOutlined className="h-5 w-5 text-red-500" />
                  <p className="text-sm text-red-500">{formError}</p>
                </>
              )}
            </div>
          </Form.Item>
        </Form>
      </Col>
    </Row>
  );
}

function LoginButton() {
  const { pending } = useFormStatus();

  return (
    <Button
      aria-disabled={pending}
      className="w-full"
      disabled={pending}
      htmlType="submit"
      type="primary"
    >
      Log in
    </Button>
  );
}
