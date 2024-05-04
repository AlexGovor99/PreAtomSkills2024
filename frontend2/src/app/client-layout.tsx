"use client";

import React from "react";
import { Avatar, Breadcrumb, Button, Dropdown, Flex, Layout, Menu, theme } from "antd";
import { UserOutlined } from "@ant-design/icons";
import { logout, useUser } from "../../user";
import { usePathname } from "next/navigation";
import { AuthGuard } from "../../auth-guard";
import dynamic from "next/dynamic";
import Link from "next/link";

const { Header, Content, Footer } = Layout;

// const items = new Array(3).fill(null).map((_, index) => ({
//   key: String(index + 1),
//   label: `nav ${index + 1}`,
// }));

const items = [] as any[];

const ClientLayout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const pathname = usePathname();

  const user = useUser();

  return (
    <AuthGuard>
      <Layout>
        {user ? (
          <Header
            style={{
              position: "sticky",
              top: 0,
              zIndex: 1,
              width: "100%",
              display: "flex",
              alignItems: "center",
            }}
          >
            <Flex justify="space-between" align="center" style={{ width: "100%" }}>
              <div style={{ flex: "1" }}>
                <div className="demo-logo" />
                <Menu
                  theme="dark"
                  mode="horizontal"
                  defaultSelectedKeys={["2"]}
                  items={items}
                  style={{ flex: 1, minWidth: 0 }}
                />
                {user?.role === "scientist" ? (
                  <Link href="/reports/new">
                    <Button type="primary">New report</Button>
                  </Link>
                ) : null}
              </div>
              <Dropdown
                menu={{
                  style: {
                    minWidth: "100px",
                  },
                  items: [
                    {
                      label: (
                        <Button
                          type="link"
                          onClick={() => {
                            logout();
                          }}
                        >
                          Log out
                        </Button>
                      ),
                      key: "0",
                    },
                  ],
                }}
                trigger={["click"]}
              >
                <Avatar
                  size="large"
                  icon={<UserOutlined />}
                  style={{
                    cursor: "pointer",
                  }}
                />
              </Dropdown>
            </Flex>
          </Header>
        ) : null}
        <Content style={{ padding: "0 48px" }}>
          <div
            style={{
              padding: 24,
              minHeight: 380,
              background: colorBgContainer,
              borderRadius: borderRadiusLG,
            }}
          >
            {children}
          </div>
        </Content>
        <Footer style={{ textAlign: "center" }}>
          Ant Design ©{new Date().getFullYear()} Created by Ant UED
        </Footer>
      </Layout>
    </AuthGuard>
  );
};

export default dynamic(async () => ClientLayout, { ssr: false });
